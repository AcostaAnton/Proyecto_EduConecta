using EduConecta.Domain.Entities;
using EduConecta.Domain.Exceptions;
using EduConecta.Domain.Validation;
using EduConecta.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EduConecta.Application.Services;

public class SesionService
{
    private readonly MongoDbContext _context;

    public SesionService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<SesionTutoria> AgendarSesionAsync(SesionTutoria nuevaSesion)
    {
        // 1. Duración entre 30 y 180 minutos
        SesionValidator.ValidarDuracion(nuevaSesion.DuracionMinutos);

        // 2. El tutor debe existir y debe impartir esa materia
        var tutor = await _context.Tutores.Find(t => t.Id == nuevaSesion.TutorId).FirstOrDefaultAsync();
        if (tutor is null)
            throw new ReglaDeNegocioException($"No existe un tutor con id '{nuevaSesion.TutorId}'.");

        SesionValidator.ValidarMateriaImpartida(tutor, nuevaSesion.Materia);

        // 3. El estudiante debe existir
        var estudiante = await _context.Estudiantes.Find(e => e.Id == nuevaSesion.EstudianteId).FirstOrDefaultAsync();
        if (estudiante is null)
            throw new ReglaDeNegocioException($"No existe un estudiante con id '{nuevaSesion.EstudianteId}'.");

        // 4. El tutor no puede tener dos sesiones en el mismo horario
        var sesionesDelTutor = await _context.Sesiones.Find(s => s.TutorId == nuevaSesion.TutorId).ToListAsync();
        SesionValidator.ValidarSinSolapamiento(
            sesionesDelTutor, nuevaSesion.Horario, nuevaSesion.DuracionMinutos,
            "El tutor ya tiene una sesión agendada en ese horario.");

        // 5. El estudiante no puede tener dos sesiones en el mismo horario (aunque sea con otro tutor)
        var sesionesDelEstudiante = await _context.Sesiones.Find(s => s.EstudianteId == nuevaSesion.EstudianteId).ToListAsync();
        SesionValidator.ValidarSinSolapamiento(
            sesionesDelEstudiante, nuevaSesion.Horario, nuevaSesion.DuracionMinutos,
            "El estudiante ya tiene una sesión agendada en ese horario.");

        nuevaSesion.Estado = EstadoSesion.Agendada;
        await _context.Sesiones.InsertOneAsync(nuevaSesion);
        return nuevaSesion;
    }

    public async Task<SesionTutoria> CompletarSesionAsync(string sesionId)
    {
        var sesion = await _context.Sesiones.Find(s => s.Id == sesionId).FirstOrDefaultAsync();
        if (sesion is null)
            throw new ReglaDeNegocioException($"No existe una sesión con id '{sesionId}'.");

        // La fecha/hora de la sesión ya debe haber pasado
        SesionValidator.ValidarSesionYaPaso(sesion.Horario);

        var actualizacion = Builders<SesionTutoria>.Update.Set(s => s.Estado, EstadoSesion.Completada);
        await _context.Sesiones.UpdateOneAsync(s => s.Id == sesionId, actualizacion);

        sesion.Estado = EstadoSesion.Completada;
        return sesion;
    }

    public async Task<List<SesionTutoria>> ListarPorEstudianteAsync(string estudianteId)
    {
        return await _context.Sesiones.Find(s => s.EstudianteId == estudianteId).ToListAsync();
    }
}