using EduConecta.Domain.Entities;
using EduConecta.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EduConecta.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Tutor> Tutores => _database.GetCollection<Tutor>("tutores");
    public IMongoCollection<Estudiante> Estudiantes => _database.GetCollection<Estudiante>("estudiantes");
    public IMongoCollection<SesionTutoria> Sesiones => _database.GetCollection<SesionTutoria>("sesiones");
}