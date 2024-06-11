using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;

namespace Mottu.Application.Deliverymen.Queries
{
    public record GetDeliverymanNotificationsQuery(string UserId) : IRequest<List<Notification>>;

    public class GetDeliverymanNotificationsQueryHandler : IRequestHandler<GetDeliverymanNotificationsQuery, List<Notification>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public GetDeliverymanNotificationsQueryHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<List<Notification>> Handle(GetDeliverymanNotificationsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId) || !Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new Exception("Invalid user identity.");
            }

            var query = new GetDeliverymanIdByUserIdQuery(userId);

            var deliverymanId = await _mediator.Send(query, cancellationToken);

            if (deliverymanId == null)
            {
                throw new Exception("Delivery person not found.");
            }
            return await _context.Notifications
                .Where(n => n.DeliverymanId == deliverymanId)
                .ToListAsync(cancellationToken);
        }
    }
}
