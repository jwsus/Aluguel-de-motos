using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mottu.Infrastructure.Data;

namespace Mottu.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Rental rental)
        {
            _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
            return rental.Id;
        }

      public async Task UpdateRentalCostAsync(Guid rentalId, DateTime endDate, decimal totalCost, CancellationToken cancellationToken)
      {
          // Buscar a entidade existente
            var rental = await _context.Rentals.FindAsync(new object[] { rentalId }, cancellationToken);
            if (rental == null)
            {
                throw new InvalidOperationException("Rental not found.");
            }

            // Atualizar as propriedades necessárias
            rental.EndDate = endDate;
            rental.TotalCost = totalCost;

            // Marcar a entidade como modificada
            _context.Rentals.Update(rental);

            await _context.SaveChangesAsync(cancellationToken);
      }
    }
}
