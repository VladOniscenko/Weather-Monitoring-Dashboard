using Weather.Domain.Entities;

namespace Weather.Application.Common.Interfaces;

public interface ITokenService
{
    Dictionary<string, string> CreateToken(User user);
}