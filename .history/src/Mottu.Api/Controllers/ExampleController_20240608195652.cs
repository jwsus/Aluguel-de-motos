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
        public ExampleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Policy = "DeliverymanPolicy")]
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok($"chechel entity with ID: {id}");
        }

    }
}
