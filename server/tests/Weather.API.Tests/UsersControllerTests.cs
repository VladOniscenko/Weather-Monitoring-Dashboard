using Microsoft.AspNetCore.Mvc;
using Moq;
using Weather.API.Controllers;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Domain.Enums;
using Weather.Application.Common.Models; 

namespace Weather.API.Tests.Controllers;

[TestClass]
public class UsersControllerTests
{
    private Mock<IUserService> _mockUserService;
    private UsersController _controller;

    [TestInitialize]
    public void Setup()
    {
        // 1. Create the fake service (The Mock)
        _mockUserService = new Mock<IUserService>();

        // 2. Inject the fake service into the real Controller
        _controller = new UsersController(_mockUserService.Object);
    }

    [TestMethod]
    public async Task GetUser_WhenUserExists_ShouldReturnOkWithData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new UserDto(userId, "Oni", "oni@email.com", DateTime.UtcNow, UserRole.User);

        // Tell the mock: "When someone calls GetByIdAsync, return this user"
        _mockUserService.Setup(s => s.GetByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult, "Result should be 200 OK");
        
        // Unwrap the ApiResponse<T>
        var response = okResult.Value as ApiResponse<UserDto>;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
        Assert.AreEqual("Oni", response.Data.Name);
    }

    [TestMethod]
    public async Task GetUser_WhenUserNull_ShouldReturnNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Tell the mock: "Return null, as if user doesn't exist"
        _mockUserService.Setup(s => s.GetByIdAsync(userId))
            .ReturnsAsync((UserDto?)null);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        // Inherited from BaseController: NotFoundResponse returns NotFoundObjectResult
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult, "Result should be 404 Not Found");
    }
}