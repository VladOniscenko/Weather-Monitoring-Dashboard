namespace Weather.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid Id { get; }
    bool IsAuthenticated { get; }
}