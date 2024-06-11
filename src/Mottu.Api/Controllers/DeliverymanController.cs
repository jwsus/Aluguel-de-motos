using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Deliverymen.Commands;
using Mottu.Application.Services;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverymanController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeliverymanController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
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

        

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            string userIdString = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var command = new UpdateDeliverymanPhotoCommand
            {
                UserId = userIdString,
                Photo = file
            };

            var result = await _mediator.Send(command);
            return Ok(new { FileUrl = result, Message = "File uploaded successfully to S3." });
        }
    }
}

