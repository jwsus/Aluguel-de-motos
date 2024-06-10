using Microsoft.AspNetCore.Mvc;
using Mottu.Domain.Entities;
using Mottu.Application.Common.Interfaces;
using Mottu.Application.Services;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly OrderNotificationProducer _notificationProducer;

    public OrdersController(IApplicationDbContext context, OrderNotificationProducer notificationProducer)
    {
        _context = context;
        _notificationProducer = notificationProducer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order, CancellationToken cancellationToken)
    {
        order.Id = Guid.NewGuid();
        order.Situation = OrderSituation.Available;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Notificar entregadores
        _notificationProducer.NotifyOrderCreated(order);

        return Ok(order);
    }

    [HttpPost("{orderId}/accept")]
    public async Task<IActionResult> AcceptOrder(Guid orderId, [FromBody] Guid deliverymanId, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        if (order == null || order.Situation != OrderSituation.Available)
        {
            return BadRequest("Pedido inválido ou já aceito.");
        }

        var deliveryman = await _context.Deliverymans.FindAsync(new object[] { deliverymanId }, cancellationToken);
        // if (deliveryman == null || deliveryman.HasAcceptedOrder)
        // {
        //     return BadRequest("Entregador inválido ou já aceitou um pedido.");
        // }

        order.Situation = OrderSituation.Accepted;
        // deliveryman.HasAcceptedOrder = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(order);
    }

    [HttpPost("{orderId}/complete")]
    public async Task<IActionResult> CompleteOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        if (order == null ||  order.Situation != OrderSituation.Accepted)
        {
            return BadRequest("Pedido inválido ou não está aceito.");
        }

        order.Situation = OrderSituation.Delivered;

        // var deliveryman = await _context.Deliverymans
        //     .FirstOrDefaultAsync(dm => dm.HasAcceptedOrder && dm.Id == orderId, cancellationToken);
        // if (deliveryman != null)
        // {
        //     deliveryman.HasAcceptedOrder = false;
        // }

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(order);
    }
}
