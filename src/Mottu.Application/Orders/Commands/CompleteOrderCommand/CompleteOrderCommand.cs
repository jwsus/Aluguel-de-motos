using MediatR;
using System;

namespace Mottu.Application.Orders.Commands
{
    public record CompleteOrderCommand(Guid OrderId, string UserId) : IRequest<CompleteOrderResponse>;

    public class CompleteOrderResponse
    {
        public Guid OrderId { get; set; }
        public OrderSituation Status { get; set; }
        public string UserId { get; set; }
    }
}
