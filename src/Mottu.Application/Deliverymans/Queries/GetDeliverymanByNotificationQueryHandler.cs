
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;

namespace Mottu.Application.Deliverymen.Queries
{
    public record GetDeliverymanByNotificationQuery(Guid UserId, Guid OrderId) : IRequest<bool>;
    public class GetDeliverymanByNotificationQueryHandler : IRequestHandler<GetDeliverymanByNotificationQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public GetDeliverymanByNotificationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(GetDeliverymanByNotificationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Deliverymans
                                 .AnyAsync(d => d.UserId == request.UserId &&
                                             _context.Notifications.Any(n => n.DeliverymanId == d.Id && n.OrderId == request.OrderId),
                                            cancellationToken);
        }
    }
}
