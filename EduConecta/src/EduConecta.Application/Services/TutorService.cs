using EduConecta.Domain.Entities;
using EduConecta.Domain.Exceptions;
using EduConecta.Domain.Validation;
using EduConecta.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EduConecta.Application.Services;

public class TutorService
{
    private readonly MongoDbContext _context;

    public TutorService(MongoDbContext context)
    {
        _context = context;
    }

    // Registrar un tutor con sus materias
    public async Task<Tutor> RegistrarTutorAsync(Tutor tutor)
    {
        await _context.Tutores.InsertOneAsync(tutor);
        return tutor;
    }

    // Buscar tutores por materia
    public async Task<List<Tutor>> BuscarPorMateriaAsync(string materia)
    {
        var filtro = Builders<Tutor>.Filter.AnyEq(t => t.Materias, materia);
        return await _context.Tutores.Find(filtro).ToListAsync();
    }

    // Listar todos los tutores
    public async Task<List<Tutor>> ListarTodosAsync()
    {
        return await _context.Tutores.Find(x => true).ToListAsync();
    }

    // Eliminar un tutor (valida que no tenga sesiones futuras)
    public async Task EliminarTutorAsync(string tutorId)
    {
        var sesionesDelTutor = await _context.Sesiones
            .Find(s => s.TutorId == tutorId)
            .ToListAsync();

        SesionValidator.ValidarTutorSinSesionesFuturas(sesionesDelTutor);

        var resultado = await _context.Tutores.DeleteOneAsync(t => t.Id == tutorId);
        if (resultado.DeletedCount == 0)
        {
            throw new ReglaDeNegocioException($"No existe un tutor con id '{tutorId}'.");
        }
    }
}