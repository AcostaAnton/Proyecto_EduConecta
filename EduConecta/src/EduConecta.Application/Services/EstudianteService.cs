using EduConecta.Domain.Entities;
using EduConecta.Infrastructure.Persistence;

namespace EduConecta.Application.Services;

public class EstudianteService
{
    private readonly MongoDbContext _context;

    public EstudianteService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Estudiante> RegistrarEstudianteAsync(Estudiante estudiante)
    {
        await _context.Estudiantes.InsertOneAsync(estudiante);
        return estudiante;
    }
}