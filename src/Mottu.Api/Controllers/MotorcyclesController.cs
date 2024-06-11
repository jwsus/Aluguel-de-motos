using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Motorcycles.Commands;
using Mottu.Application.Motorcycles.CreateMotorcycle.Commands;
using Mottu.Application.Motorcycles.Queries;
using Mottu.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations; // Importação para anotações do Swagger

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MotorcyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new motorcycle", Description = "Creates a new motorcycle with the specified details. Only Admin")]
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
        [Authorize(Policy = "AdminPolicy, DeliverymanPolicy")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get motorcycle by ID", Description = "Retrieves the details of a motorcycle by its ID.")]
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
        [Authorize(Policy = "AdminPolicy, DeliverymanPolicy")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get motorcycles", Description = "Retrieves a list of motorcycles, optionally filtered by plate.")]
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("/plate")]
        [SwaggerOperation(Summary = "Update motorcycle plate", Description = "Updates the plate of an existing motorcycle.")]
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete motorcycle", Description = "Deletes a motorcycle by its ID.")]
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
