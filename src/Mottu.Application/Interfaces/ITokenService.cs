using Mottu.Domain.Entities;

namespace Mottu.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
