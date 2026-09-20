using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EduConecta.Domain.Entities;

public class Estudiante
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
}