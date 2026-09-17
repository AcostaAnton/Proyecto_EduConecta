using EduConecta.Application.Services;
using EduConecta.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduConecta.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutoresController : ControllerBase
{
    private readonly TutorService _tutorService;

    public TutoresController(TutorService tutorService)
    {
        _tutorService = tutorService;
    }

    // POST /api/tutores
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] Tutor tutor)
    {
        var creado = await _tutorService.RegistrarTutorAsync(tutor);
        return CreatedAtAction(nameof(BuscarPorMateria), new { materia = creado.Materias.FirstOrDefault() }, creado);
    }

    // GET /api/tutores?materia=Matematicas
    [HttpGet]
    public async Task<IActionResult> BuscarPorMateria([FromQuery] string materia)
    {
        var tutores = await _tutorService.BuscarPorMateriaAsync(materia);
        return Ok(tutores);
    }

    // DELETE /api/tutores/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(string id)
    {
        await _tutorService.EliminarTutorAsync(id);
        return NoContent();
    }
}