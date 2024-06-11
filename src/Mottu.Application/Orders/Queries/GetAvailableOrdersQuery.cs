using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;

public record GetAvailableOrdersQuery : IRequest<List<OrderDto>>;

public class OrderDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal RideValue { get; set; }
    public OrderSituation Situation { get; set; }
}
public class GetAvailableOrdersQueryHandler : IRequestHandler<GetAvailableOrdersQuery, List<OrderDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAvailableOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> Handle(GetAvailableOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .Where(o => o.Situation == OrderSituation.Available)
                             .Select(o => new OrderDto
                             {
                                 Id = o.Id,
                                 CreatedAt = o.CreatedAt,
                                 RideValue = o.RideValue,
                                 Situation = o.Situation,
                             })
                             .ToListAsync(cancellationToken);
    }
}
