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
        var actionResult = await _controller.GetUser(userId);

        // Assert
        Assert.IsNotNull(actionResult.Result);
        Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));

        var okResult = (OkObjectResult)actionResult.Result;
        Assert.IsInstanceOfType(okResult.Value, typeof(ApiResponse<UserDto>));

        var response = (ApiResponse<UserDto>)okResult.Value;

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
        var actionResult = await _controller.GetUser(userId);

        // Assert
        Assert.IsNotNull(actionResult.Result);
        Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult));
    }
}