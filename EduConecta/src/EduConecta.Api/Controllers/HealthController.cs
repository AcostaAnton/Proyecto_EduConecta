using EduConecta.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EduConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly MongoDbContext _context;

    public HealthController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet("mongo")]
    public async Task<IActionResult> CheckMongo()
    {
        var cantidad = await _context.Tutores.CountDocumentsAsync(FilterDefinition<EduConecta.Domain.Entities.Tutor>.Empty);
        return Ok(new { conectado = true, tutoresRegistrados = cantidad });
    }
}