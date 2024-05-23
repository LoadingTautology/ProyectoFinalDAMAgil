using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Horario
{
    public interface IHorarioService
    {
        Task<HorarioModel> CreateHorario(HorarioModel horario);
        Task<HorarioModel> ReadHorario(int idAula, int idDiaFranja);
        Task<HorarioModel> UpdateHorario(HorarioModel horario);
        Task<HorarioModel> DeleteHorario(HorarioModel horario);

        Task<IEnumerable<HorarioModel>> ListHorariosEstudio(int idEstudio);

        Task<bool> ExistHorario(HorarioModel horario);
        Task<bool> ExistHorarioSinAula(HorarioModel horario);

        Task<HorarioModel> CambiarColor(HorarioModel horario);

    }
}
