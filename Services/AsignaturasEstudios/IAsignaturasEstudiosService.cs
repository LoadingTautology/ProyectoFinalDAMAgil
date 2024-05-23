using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.AsignaturasEstudios
{
    public interface IAsignaturasEstudiosService
    {
        Task<AsignaturasEstudiosModel> ReadAsignaturasEstudios(int idAsignaturasEstudios);
        Task<AsignaturasEstudiosModel> ReadIdAsignaturasEstudios(int idAsignatura, int idEstudio);
    }
}
