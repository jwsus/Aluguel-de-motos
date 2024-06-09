using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.CreateMotorcycle.Commands;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Domain.Entities;
using System.Threading.Tasks;

namespace Mottu.Api.Controllers
{
    // [Authorize(Policy = "DeliverymanPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MotorcyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _mediator.Send(command);
                return Ok(id);
                // return CreatedAtAction(nameof(GetMotorcycleById), new { id }, command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleById(Guid id)
        {
            var query = new GetMotorcycleByIdQuery(id);
            var motorcycle = await _mediator.Send(query);

            if (motorcycle == null)
            {
                return NotFound();
            }

            return Ok(motorcycle);
        }

        [HttpGet]
        public async Task<ActionResult<List<Motorcycle>>> GetMotorcycles([FromQuery] string? plate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetMotorcyclesByPlateQuery
            {
                Plate = plate,
                Page = page,
                PageSize = pageSize
            };

            var motorcycles = await _mediator.Send(query);
            return Ok(motorcycles);
        }

        [HttpPut("/plate")]
        public async Task<IActionResult> UpdatePlate([FromBody] UpdateMotorcyclePlateCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycle(Guid id)
        {
            try
            {
                var command = new DeleteMotorcycleCommand { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
