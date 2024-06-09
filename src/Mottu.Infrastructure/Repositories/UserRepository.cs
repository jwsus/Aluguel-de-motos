using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Data;
using System.Threading.Tasks;

namespace Mottu.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
