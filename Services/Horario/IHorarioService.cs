using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Horario
{
    public interface IHorarioService
    {
        Task<HorarioModel> CreateHorario(HorarioModel horario);
        Task<HorarioModel> ReadHorario(int idDiaFranja, int idAsignatura, int idEstudio);
        Task<HorarioModel> UpdateHorario(HorarioModel horario);
        Task<HorarioModel> DeleteHorario(HorarioModel horario);

        Task<IEnumerable<HorarioModel>> ListHorariosEstudio(int idEstudio);
        Task<IEnumerable<HorarioModel>> ListHorariosEstudioCursoAsignatura(int cursoAsignatura, int idEstudio);
        Task<IEnumerable<HorarioModel>> ListHorariosAsignaturaEstudio(int idAsignatura, int idEstudio);

        Task<bool> ExistHorario(int idAula, int idDiaFranja);
        Task<bool> ExistHorario(int idDiaFranja, int idAsignatura, int idEstudio);
        Task<bool> ExistHorario(HorarioModel horario);

        Task<HorarioModel> CambiarColor(HorarioModel horario);

    }
}
