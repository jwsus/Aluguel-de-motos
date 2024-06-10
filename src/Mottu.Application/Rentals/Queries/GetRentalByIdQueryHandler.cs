using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Rentals.Handlers
{
    public record GetRentalByIdQuery(Guid rentalId) : IRequest<Rental>;
    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, Rental>
    {
        private readonly IApplicationDbContext _context;

        public GetRentalByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.Id == request.rentalId, cancellationToken);
        }
    }
}
