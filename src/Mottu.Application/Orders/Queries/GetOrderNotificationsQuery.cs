using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;

public record GetOrderNotificationsQuery(Guid OrderId) : IRequest<List<NotificationDto>>;
public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid DeliverymanId { get; set; }
    public DateTime NotifiedAt { get; set; }
    public string DeliverymanName { get; set; }
}
public class GetOrderNotificationsQueryHandler : IRequestHandler<GetOrderNotificationsQuery, List<NotificationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrderNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NotificationDto>> Handle(GetOrderNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Notifications
                             .Where(n => n.OrderId == request.OrderId)
                             .Select(n => new NotificationDto
                             {
                                 Id = n.Id,
                                 OrderId = n.OrderId,
                                 DeliverymanId = n.DeliverymanId,
                                 NotifiedAt = n.CreatedAt,
                                 DeliverymanName = _context.Deliverymans
                                                  .Where(d => d.Id == n.DeliverymanId)
                                                  .Select(d => d.Name)
                                                  .FirstOrDefault()
                             })
                             .ToListAsync(cancellationToken);
    }
}
