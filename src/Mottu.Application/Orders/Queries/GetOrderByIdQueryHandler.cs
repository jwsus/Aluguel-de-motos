using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mottu.Application.Orders.Queries
{
    public record GetOrderByIdQuery(Guid OrderId) : IRequest<Order>;
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                                      .Where(o => o.Id == request.OrderId)
                                      .FirstOrDefaultAsync(cancellationToken);

            return order;
        }
    }
}
