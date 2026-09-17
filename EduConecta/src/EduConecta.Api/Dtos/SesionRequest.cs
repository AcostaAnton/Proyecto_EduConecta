namespace EduConecta.Api.Dtos;

public class SesionRequest
{
    public string TutorId { get; set; } = string.Empty;
    public string EstudianteId { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public DateTime Horario { get; set; }
    public int DuracionMinutos { get; set; }
}