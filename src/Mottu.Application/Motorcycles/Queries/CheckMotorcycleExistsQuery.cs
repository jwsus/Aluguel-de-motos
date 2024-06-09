using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Motorcycles.Queries
{
    public record CheckMotorcycleExistsQuery(Guid Id) : IRequest<bool>;
    public class CheckMotorcycleExistsQueryHandler : IRequestHandler<CheckMotorcycleExistsQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckMotorcycleExistsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckMotorcycleExistsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Motorcycles
                                 .AnyAsync(m => m.Id == request.Id, cancellationToken);
        }
    }
}
