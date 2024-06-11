using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Deliverymen.Queries
{
    public record CheckDeliverymanEligibledOrdersQuery(Guid DeliverymanId) : IRequest<bool>;
    public class CheckDeliverymanEligibledOrdersQueryHandler : IRequestHandler<CheckDeliverymanEligibledOrdersQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public CheckDeliverymanEligibledOrdersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CheckDeliverymanEligibledOrdersQuery request, CancellationToken cancellationToken)
        {
            return !await _context.Orders
                .AnyAsync(o => o.DeliverymanId == request.DeliverymanId 
                            && o.Situation == OrderSituation.Accepted
                            && _context.Rentals.Any(r => r.DeliverymanId == o.DeliverymanId && r.EndDate == null), 
                        cancellationToken);

        }
    }
}
