namespace Weather.Domain.Entities;
using Weather.Domain.Enums;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; set; } = UserRole.User;
}