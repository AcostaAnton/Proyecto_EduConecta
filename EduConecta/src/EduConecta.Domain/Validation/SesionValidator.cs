using EduConecta.Domain.Entities;
using EduConecta.Domain.Exceptions;

namespace EduConecta.Domain.Validation;

public static class SesionValidator
{
    private const int DuracionMinima = 30;
    private const int DuracionMaxima = 180;

    // Regla: la duración debe estar entre 30 y 180 minutos
    public static void ValidarDuracion(int duracionMinutos)
    {
        if (duracionMinutos < DuracionMinima || duracionMinutos > DuracionMaxima)
        {
            throw new ReglaDeNegocioException(
                $"La duración de la sesión debe estar entre {DuracionMinima} y {DuracionMaxima} minutos.");
        }
    }

    // Regla: no se puede agendar una materia que el tutor no imparte
    public static void ValidarMateriaImpartida(Tutor tutor, string materia)
    {
        if (!tutor.Materias.Contains(materia, StringComparer.OrdinalIgnoreCase))
        {
            throw new ReglaDeNegocioException(
                $"El tutor '{tutor.Nombre}' no imparte la materia '{materia}'.");
        }
    }

    // Reglas: ni el tutor ni el estudiante pueden tener dos sesiones en el mismo horario
    // (se usa la misma lógica para ambos casos, solo cambia qué lista de sesiones le pasas)
    public static void ValidarSinSolapamiento(
        IEnumerable<SesionTutoria> sesionesExistentes,
        DateTime horarioNuevo,
        int duracionNuevaMinutos,
        string mensajeError)
    {
        var finNuevo = horarioNuevo.AddMinutes(duracionNuevaMinutos);

        foreach (var sesion in sesionesExistentes)
        {
            if (sesion.Estado == EstadoSesion.Completada) continue;

            var finExistente = sesion.Horario.AddMinutes(sesion.DuracionMinutos);
            bool seSolapan = horarioNuevo < finExistente && sesion.Horario < finNuevo;

            if (seSolapan)
            {
                throw new ReglaDeNegocioException(mensajeError);
            }
        }
    }

    // Regla: no se puede completar una sesión cuya fecha/hora todavía no ha pasado
    public static void ValidarSesionYaPaso(DateTime horario)
    {
        if (horario > DateTime.UtcNow)
        {
            throw new ReglaDeNegocioException(
                "No se puede completar una sesión cuya fecha/hora todavía no ha pasado.");
        }
    }

    // Regla: no se puede eliminar un tutor con sesiones futuras agendadas
    public static void ValidarTutorSinSesionesFuturas(IEnumerable<SesionTutoria> sesionesDelTutor)
    {
        bool tieneSesionesFuturas = sesionesDelTutor.Any(s =>
            s.Estado == EstadoSesion.Agendada && s.Horario > DateTime.UtcNow);

        if (tieneSesionesFuturas)
        {
            throw new ReglaDeNegocioException(
                "No se puede eliminar un tutor que tiene sesiones futuras agendadas.");
        }
    }
}