using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Rentals.Commands;
using MediatR;
using Swashbuckle.AspNetCore.Annotations; // Importação para anotações do Swagger

namespace Mottu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentalsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new rental", Description = "Creates a new rental for the authenticated deliveryman.")]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalCommand command)
        {
            string userIdString = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid deliverymanId))
            {
                return BadRequest("Invalid user identity.");
            }

            command.SetDeliverymanId(deliverymanId);

            if (command == null)
            {
                return BadRequest("Invalid rental command.");
            }

            try
            {
                var rental = await _mediator.Send(command);
                return Ok(rental);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("return")]
        [SwaggerOperation(Summary = "Return a rental", Description = "Marks the rental as returned for the specified rental ID.")]
        public async Task<IActionResult> ReturnRental([FromBody] ReturnRentalCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid return rental command.");
            }

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
