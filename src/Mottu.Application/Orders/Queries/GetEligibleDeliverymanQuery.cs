using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;

namespace Mottu.Application.Orders.Queries
{
    public record GetEligibleDeliverymanQuery() : IRequest<List<Deliveryman>>;

    public class GetEligibleDeliverymanQueryHandler : IRequestHandler<GetEligibleDeliverymanQuery, List<Deliveryman>>
    {
        private readonly IApplicationDbContext _context;

        public GetEligibleDeliverymanQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Deliveryman>> Handle(GetEligibleDeliverymanQuery request, CancellationToken cancellationToken)
        {
            var deliverymen = await _context.Deliverymans
                .Where(d => !d.Orders.Any(o => o.Situation == OrderSituation.Accepted) &&
                            _context.Rentals.Any(r => r.DeliverymanId == d.Id && r.EndDate == null))
                .ToListAsync(cancellationToken);

            return deliverymen;
        }
    }
}
