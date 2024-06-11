using MediatR;
using Mottu.Application.Orders.Commands;
using Mottu.Application.Orders.Queries;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Repositories;

namespace Mottu.Application.Orders.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeliverymanRepository _deliverymanRepository;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler
        (
          IOrderRepository orderRepository, 
          IDeliverymanRepository deliverymanRepository,
          INotificationRepository notificationRepository,
          IMediator mediator
        )
        {
            _orderRepository = orderRepository;
            _deliverymanRepository = deliverymanRepository;
            _notificationRepository = notificationRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                RideValue = request.Value,
                Situation = OrderSituation.Available
            };

            await _orderRepository.AddOrderAsync(order);

            var query = new GetEligibleDeliverymanQuery();
            var eligibleDeliverymen =  await _mediator.Send(query);

            var notifications = eligibleDeliverymen.Select(deliveryman => new Notification
            {
                OrderId = order.Id,
                DeliverymanId = deliveryman.Id,
                Message = "New order available",
            }).ToList();

            await _notificationRepository.AddNotificationsAsync(notifications);
            foreach (var deliveryman in eligibleDeliverymen)
            {
                // Simula a notificação
                Console.WriteLine($"Notifying deliveryman {deliveryman.Id} about new order {order.Id}");
                
            }

            return order.Id;
        }
    }
}
