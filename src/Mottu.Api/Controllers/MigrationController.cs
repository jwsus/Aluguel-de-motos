using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mottu.Application.Common.Interfaces;
using Mottu.Infrastructure.Data;

namespace SeuProjeto.Controllers // Substitua pelo namespace correto do seu projeto
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationsController : ControllerBase
    {
        private readonly ApplicationDbContext  _context;

        public MigrationsController(ApplicationDbContext  context)
        {
            _context = context;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateDatabase()
        {
            try
            {
                await _context.Database.MigrateAsync();
                return Ok("Database updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
