using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Motorcycles.Queries
{
    public record GetMotorcycleByIdQuery(Guid Id) : IRequest<Motorcycle>;

    public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, Motorcycle>
    {
        private readonly IApplicationDbContext _context;

        public GetMotorcycleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Motorcycle> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Motorcycles
                                 .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
        }
    }
}
