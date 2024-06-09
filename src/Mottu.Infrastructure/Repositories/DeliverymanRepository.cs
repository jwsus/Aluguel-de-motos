using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Data;
using System.Threading.Tasks;

namespace Mottu.Infrastructure.Repositories
{
    public class DeliverymanRepository : IDeliverymanRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliverymanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Deliveryman> GetDeliverymanByUserIdAsync(Guid userId)
        {
            return await _context.Deliverymans
                                 .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task AddDeliverymanAsync(Deliveryman deliveryman)
        {
            await _context.Deliverymans.AddAsync(deliveryman);
            await _context.SaveChangesAsync();
        }
    }
}
