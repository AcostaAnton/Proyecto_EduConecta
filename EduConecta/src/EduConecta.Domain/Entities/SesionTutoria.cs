using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EduConecta.Domain.Entities;

public enum EstadoSesion
{
    Agendada,
    Completada
}

public class SesionTutoria
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("tutorId")]
    public string TutorId { get; set; } = string.Empty;

    [BsonElement("estudianteId")]
    public string EstudianteId { get; set; } = string.Empty;

    [BsonElement("materia")]
    public string Materia { get; set; } = string.Empty;

    [BsonElement("horario")]
    public DateTime Horario { get; set; }

    [BsonElement("duracionMinutos")]
    public int DuracionMinutos { get; set; }

    [BsonElement("estado")]
    [BsonRepresentation(BsonType.String)]
    public EstadoSesion Estado { get; set; } = EstadoSesion.Agendada;
}