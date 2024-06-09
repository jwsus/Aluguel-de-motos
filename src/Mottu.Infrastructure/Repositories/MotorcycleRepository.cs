using Microsoft.EntityFrameworkCore;
using Mottu.Application.Interfaces;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Data;
using System.Threading.Tasks;

namespace Mottu.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly ApplicationDbContext _context;

        public MotorcycleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LicensePlateExistsAsync(string licensePlate)
        {
            return await _context.Motorcycles.AnyAsync(m => m.LicensePlate == licensePlate);
        }

        public async Task<Guid> AddAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle.Id;
        }

      public async Task UpdatePlateAsync(Guid id, string newPlate)
        {
            var motorcycle = new Motorcycle
            {
                Id = id,
                LicensePlate = newPlate
            };

            _context.Motorcycles.Attach(motorcycle);
            _context.Entry(motorcycle).Property(m => m.LicensePlate).IsModified = true;

            await _context.SaveChangesAsync();
        }
  }
}
