using EduConecta.Application.Services;
using EduConecta.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly EstudianteService _estudianteService;
    private readonly SesionService _sesionService;

    public EstudiantesController(EstudianteService estudianteService, SesionService sesionService)
    {
        _estudianteService = estudianteService;
        _sesionService = sesionService;
    }

    // POST /api/estudiantes
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Estudiante estudiante)
    {
        var creado = await _estudianteService.RegistrarEstudianteAsync(estudiante);
        return Ok(creado);
    }

    // GET /api/estudiantes/{id}/sesiones
    [HttpGet("{id}/sesiones")]
    public async Task<IActionResult> ListarSesiones(string id)
    {
        var sesiones = await _sesionService.ListarPorEstudianteAsync(id);
        return Ok(sesiones);
    }
}