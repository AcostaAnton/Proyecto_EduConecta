using EduConecta.Domain.Entities;
using EduConecta.Infrastructure.Persistence;
using MongoDB.Driver;

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

    // Listar todos los estudiantes
    public async Task<List<Estudiante>> ListarTodosAsync()
    {
        return await _context.Estudiantes.Find(x => true).ToListAsync();
    }
}