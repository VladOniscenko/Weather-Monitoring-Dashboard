using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Interfaces;
using Weather.Application.Services;
using Weather.Domain.Entities;
using Weather.Domain.Enums;

namespace Weather.Application.Tests.Services;

[TestClass]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockRepo;
    private Mock<ITokenService> _mockTokenService;
    private Mock<ICurrentUserService> _mockCurrentUser;

    private UserService _userService;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _mockCurrentUser = new Mock<ICurrentUserService>();

        _userService = new UserService(
            _mockRepo.Object,
            _mockTokenService.Object,
            _mockCurrentUser.Object
        );
    }

    [TestMethod]
    public async Task RegisterAsync_WithValidData_ShouldCreateUserAndReturnDto()
    {
        // Arrange
        var request = new UserRegisterRequestDto("New User", "new@test.com", "password123");
        Console.WriteLine(request);

        _mockRepo.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .ReturnsAsync(false);

        // Act
        var result = await _userService.RegisterAsync(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(request.Email, result.Email);
        
        // Verify Repository was called to save
        _mockRepo.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == request.Email)), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task RegisterAsync_WithInvalidEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new UserRegisterRequestDto("Name", "invalid-email", "pass");

        // Act
        await _userService.RegisterAsync(request);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), "User already exists.")]
    public async Task RegisterAsync_WhenEmailTaken_ShouldThrowException()
    {
        // Arrange
        var request = new UserRegisterRequestDto("Name", "taken@test.com", "pass");

        _mockRepo.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .ReturnsAsync(true);

        // Act
        await _userService.RegisterAsync(request);
    }

    [TestMethod]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User("test@test.com", "Test User", hashedPassword, UserRole.User);

        var request = new UserLoginRequestDto("test@test.com", password);

        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync(user);

        var fakeTokenData = new Dictionary<string, string> 
        { 
            { "token", "fake-jwt-token" }, 
            { "expiresAt", DateTime.UtcNow.AddHours(1).ToString() } 
        };
        _mockTokenService.Setup(t => t.CreateToken(user)).Returns(fakeTokenData);

        // Act
        var result = await _userService.LoginAsync(request);

        // Assert
        Assert.AreEqual("fake-jwt-token", result.Token);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task LoginAsync_WithWrongPassword_ShouldThrowUnauthorized()
    {
        // Arrange
        var realPassword = "correct_password";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(realPassword);
        var user = new User("test@test.com", "Test User", hashedPassword);

        var request = new UserLoginRequestDto("test@test.com", "WRONG_PASSWORD");

        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync(user);

        // Act
        await _userService.LoginAsync(request);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task LoginAsync_WhenUserNotFound_ShouldThrowUnauthorized()
    {
        // Arrange
        var request = new UserLoginRequestDto("unknown@test.com", "pass");

        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync((User?)null);

        // Act
        await _userService.LoginAsync(request);
    }

    [TestMethod]
    public async Task UpdateProfileAsync_ShouldUpdateEmail_WhenNotTaken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("old@test.com", "Old Name", "hash");
        var request = new UserUpdateRequestDto("New Name", "new@test.com");

        _mockCurrentUser.Setup(c => c.Id).Returns(userId);

        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync(user);

        _mockRepo.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .ReturnsAsync(false);

        // Act
        var result = await _userService.UpdateProfileAsync(request);

        // Assert
        Assert.AreEqual("New Name", result.Name);
        Assert.AreEqual("new@test.com", result.Email);
        
        _mockRepo.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public async Task UpdateProfileAsync_WhenUserNotFound_ShouldThrowKeyNotFound()
    {
        // Arrange
        _mockCurrentUser.Setup(c => c.Id).Returns(Guid.NewGuid());
        
        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync((User?)null);

        // Act
        await _userService.UpdateProfileAsync(new UserUpdateRequestDto("Name", "email@test.com"));
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("Test", "test@test.com", "hash");
        
        // ⚠️ FIX: Pass 'null'
        _mockRepo.Setup(r => r.FindOneAsync(
                It.IsAny<Expression<Func<User, bool>>>(), 
                null))
             .ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Name);
    }
}