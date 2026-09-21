using EduConecta.Api.Dtos;
using EduConecta.Application.Services;
using EduConecta.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SesionesController : ControllerBase
{
    private readonly SesionService _sesionService;

    public SesionesController(SesionService sesionService)
    {
        _sesionService = sesionService;
    }

    // POST /api/sesiones
    [HttpPost]
    public async Task<IActionResult> Agendar([FromBody] SesionRequest request)
    {
        var sesion = new SesionTutoria
        {
            TutorId = request.TutorId,
            EstudianteId = request.EstudianteId,
            Materia = request.Materia,
            Horario = request.Horario,
            DuracionMinutos = request.DuracionMinutos
        };

        var creada = await _sesionService.AgendarSesionAsync(sesion);
        return CreatedAtAction(nameof(Agendar), new { id = creada.Id }, creada);
    }

    // PATCH /api/sesiones/{id}/completar
    [HttpPatch("{id}/completar")]
    public async Task<IActionResult> Completar(string id)
    {
        var sesion = await _sesionService.CompletarSesionAsync(id);
        return Ok(sesion);
    }

    // GET /api/sesiones/todas
    [HttpGet("todas")]
    public async Task<IActionResult> ListarTodas()
    {
    var sesiones = await _sesionService.ListarTodasAsync();
    return Ok(sesiones);
    }
}