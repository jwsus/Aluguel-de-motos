using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations; 
using MediatR;
using Mottu.Application.Orders.Commands;
using Microsoft.AspNetCore.Authorization;
using Mottu.Domain.Entities;
using Mottu.Application.Deliverymen.Queries;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrdersController
    (
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new order", Description = "Creates a new order with the specified details. Only Admin")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return Ok(orderId);
    }
    [Authorize(Policy = "DeliverymanPolicy")]
    [HttpPost("/accept")]
    [SwaggerOperation(Summary = "Accept an order", Description = "Accepts the specified order and assigns it to the authenticated deliveryman.")]
    public async Task<IActionResult> AcceptOrder(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            string userIdString = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            var command = new AcceptOrderCommand(orderId, userIdString);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize(Policy = "DeliverymanPolicy")]
    [HttpPost("{orderId}/complete")]
    [SwaggerOperation(Summary = "Complete an order", Description = "Marks the specified order as delivered.")]
    public async Task<IActionResult> CompleteOrder(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            string userIdString = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var command = new CompleteOrderCommand(orderId, userIdString);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{orderId}/notifications")]
    [SwaggerOperation(Summary = "Get notifications for an order", Description = "Retrieves all notifications associated with the specified order. Only Admin")]
    public async Task<IActionResult> GetOrderNotifications(Guid orderId)
    {
        var query = new GetOrderNotificationsQuery(orderId);
        var notifications = await _mediator.Send(query);
        return Ok(notifications);
    }
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("available")]
    [SwaggerOperation(Summary = "Get available orders", Description = "Retrieves all orders that are currently available. Only Admin")]
    public async Task<IActionResult> GetAvailableOrders()
    {
        var query = new GetAvailableOrdersQuery();
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    [HttpGet("/notifications")]
    [Authorize(Policy = "DeliverymanPolicy")]
     [SwaggerOperation(Summary = "Get deliveryman notifications", Description = "Retrieves all notifications related to a specific deliveryman.")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        string userIdString = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        var query = new GetDeliverymanNotificationsQuery(userIdString);
        var notifications = await _mediator.Send(query);

        if (notifications == null || notifications.Count == 0)
        {
            return NotFound();
        }

        return Ok(notifications);
    }
}
