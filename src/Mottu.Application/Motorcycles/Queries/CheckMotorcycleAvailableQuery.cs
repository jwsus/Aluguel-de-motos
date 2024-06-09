using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;

namespace Mottu.Application.Motorcycles.Queries
{
     public record CheckMotorcycleAvailableQuery(Guid MotorcycleId) : IRequest<bool>;
    public class CheckMotorcycleAvailableQueryHandler : IRequestHandler<CheckMotorcycleAvailableQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckMotorcycleAvailableQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckMotorcycleAvailableQuery request, CancellationToken cancellationToken)
        {
            return !await _context.Rentals
                                  .AnyAsync(r => r.MotorcycleId == request.MotorcycleId && r.EndDate == null, cancellationToken);
        }
    }
}
