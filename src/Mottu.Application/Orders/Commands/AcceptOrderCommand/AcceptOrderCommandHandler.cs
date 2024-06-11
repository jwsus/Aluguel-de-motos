using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Deliverymen.Queries;
using Mottu.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Orders.Commands
{
    public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand, AcceptOrderResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public AcceptOrderCommandHandler
        (
          IApplicationDbContext context, 
          IHttpContextAccessor httpContextAccessor,
          IMediator mediator
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<AcceptOrderResponse> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (string.IsNullOrEmpty(request.UserId) || !Guid.TryParse(request.UserId, out Guid userId))
                {
                    throw new Exception("Invalid user identity.");
                }

                var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
                if (order == null || order.Situation != OrderSituation.Available)
                {
                    throw new Exception("Invalid or already accepted order.");
                }

                var query = new GetDeliverymanIdByUserIdQuery(userId);
                var deliverymanId = await _mediator.Send(query, cancellationToken);

                if (deliverymanId == null)
                {
                    throw new Exception("Delivery person not found.");
                }

                var checkAcceptedOrderQUery = new CheckDeliverymanEligibledOrdersQuery(deliverymanId);
                var eligible = await _mediator.Send(checkAcceptedOrderQUery, cancellationToken);
                if (!eligible)
                {
                    throw new Exception("Delivery person is not eligible.");
                }

                order.Situation = OrderSituation.Accepted;
                order.DeliverymanId = deliverymanId;

                await _context.SaveChangesAsync(cancellationToken);

                return new AcceptOrderResponse
                {
                    OrderId = order.Id,
                    Situation = order.Situation,
                    DeliverymanId = deliverymanId
                };
            }
            catch (Exception ex)
            {             
              throw;
            }
        }
    }
}
