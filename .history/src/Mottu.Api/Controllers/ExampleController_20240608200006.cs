using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuProjeto.Api.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExampleController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Policy = "DeliverymanPolicy")]
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId")?.Value;
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok($"chechel entity with ID: {id}");
        }

    }
}
