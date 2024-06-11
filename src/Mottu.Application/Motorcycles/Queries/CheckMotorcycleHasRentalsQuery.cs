using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Motorcycles.Queries
{
    public record CheckMotorcycleHasRentalsQuery(Guid MotorcycleId) : IRequest<bool>;
    public class CheckMotorcycleHasRentalsQueryHandler : IRequestHandler<CheckMotorcycleHasRentalsQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckMotorcycleHasRentalsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckMotorcycleHasRentalsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Rentals
                .AnyAsync(r => r.MotorcycleId == request.MotorcycleId, cancellationToken);
        }
    }
}
