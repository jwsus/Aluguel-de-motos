using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Deliverymen.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverymanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliverymanController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("register")]
        [SwaggerOperation(Summary = "Register Deliveryman", Description = "Register a new delivery person. No login required.")]
        public async Task<IActionResult> Register([FromBody] RegisterDeliverymanCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _mediator.Send(command);
                return Ok(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
