using Mottu.Domain.Entities;
using System.Threading.Tasks;

namespace Mottu.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAsync(string userName);
        Task AddUserAsync(User user);
    }
}
