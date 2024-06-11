using MediatR;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Deliverymen.Queries;
using Mottu.Application.Orders.Queries;

namespace Mottu.Application.Orders.Commands
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, CompleteOrderResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CompleteOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<CompleteOrderResponse> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId) || !Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new Exception("Invalid user identity.");
            }

            var query = new GetOrderByIdQuery(request.OrderId);
            var order = await _mediator.Send(query);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            if (order.Situation != OrderSituation.Accepted)
            {
                throw new Exception("Only accepted orders can be completed.");
            }

            
            var queryUserId = new GetDeliverymanIdByUserIdQuery(userId);
            var deliverymanId = await _mediator.Send(queryUserId, cancellationToken);

            if (deliverymanId == null)
            {
                throw new Exception("Delivery person not found.");
            }

            if(deliverymanId != order.DeliverymanId)
            {
                throw new Exception("Delivery person does not match the order");
            }

            order.Situation = OrderSituation.Delivered;

            await _context.SaveChangesAsync(cancellationToken);

            return new CompleteOrderResponse
            {
                OrderId = order.Id,
                Status = order.Situation
            };
        }
    }
}
