using MediatR;

namespace Mottu.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public decimal Value { get; set; }
    }
}
