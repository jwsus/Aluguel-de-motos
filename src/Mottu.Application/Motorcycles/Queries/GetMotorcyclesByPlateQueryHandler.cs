using MediatR;
using Mottu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mottu.Application.Common.Interfaces;

namespace Mottu.Application.Motorcycles.Queries
{
    public class GetMotorcyclesByPlateQuery : IRequest<List<Motorcycle>>
    {
        public string? Plate { get; set; }
        public int Page { get; set; } = 1; // Página padrão é 1
        public int PageSize { get; set; } = 10; // Tamanho padrão da página é 10
    }

    public class GetMotorcyclesByPlateQueryHandler : IRequestHandler<GetMotorcyclesByPlateQuery, List<Motorcycle>>
    {
        private readonly IApplicationDbContext _context;

        public GetMotorcyclesByPlateQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Motorcycle>> Handle(GetMotorcyclesByPlateQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Motorcycles.AsQueryable();

            if (!string.IsNullOrEmpty(request.Plate))
            {
                query = query.Where(m => m.LicensePlate.Contains(request.Plate));
            }

            return await query
                        .Skip((request.Page - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);
        }
    }
}
