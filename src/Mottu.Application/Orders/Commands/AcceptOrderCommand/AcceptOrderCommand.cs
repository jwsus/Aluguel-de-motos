using MediatR;
using System;

namespace Mottu.Application.Orders.Commands
{
    public record AcceptOrderCommand(Guid OrderId, string UserId) : IRequest<AcceptOrderResponse>;

    public class AcceptOrderResponse
    {
        public Guid OrderId { get; set; }
        public OrderSituation Situation { get; set; }
        public Guid UserId { get; set; }
        public Guid DeliverymanId { get; set; }
    }
}
