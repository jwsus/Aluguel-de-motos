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
        public ExampleController()
        {
           
        }
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok($"return entity with ID: {id}");
        }

    }
}
