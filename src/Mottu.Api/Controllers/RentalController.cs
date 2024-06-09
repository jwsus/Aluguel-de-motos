using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Rentals.Commands;
using MediatR;
using System.Threading.Tasks;
using System;

namespace Mottu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalCommand command)
        {
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
    }
}
