using Weather.Domain.Entities;
using Weather.Domain.Enums;

namespace Weather.Domain.Tests.Entities;

[TestClass]
public class UserTests
{
    // [MethodName]_[Scenario]_[ExpectedResult]

    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateUser()
    {
        // Act
        var user = new UserBuilder().Build();

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual("test", user.Name);
        Assert.AreEqual("test@email.com", user.Email);
        Assert.AreEqual(UserRole.User, user.Role);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithEmptyName_ShouldThrowException()
    {
        // Act
        new UserBuilder().WithName("").Build();
    }

    [TestMethod]
    public void UpdateDetails_WithValidData_ShouldUpdateAndTimestamp()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var oldUpdateDate = user.UpdatedAt;

        // Act
        user.UpdateDetails("new", "new@email.com");

        // Assert
        Assert.AreEqual("new", user.Name);
        Assert.IsNotNull(user.UpdatedAt);
        Assert.AreNotEqual(oldUpdateDate, user.UpdatedAt);
    }
}