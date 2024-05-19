using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Asignatura
{
    public interface IAsignaturaService
    {
        Task<AsignaturaModel> CreateAsignatura(AsignaturaModel asignaturaModel);
        Task<AsignaturaModel> ReadAsignatura(int idAsignatura);
        Task<AsignaturaModel> UpdateAsignatura(AsignaturaModel asignaturaModel);
        Task<AsignaturaModel> DeleteAsignatura(AsignaturaModel asignaturaModel);

        Task<IEnumerable<AsignaturaModel>> ListadoAsignatura(int idCiclo);
        Task<bool> ExistAsignatura(AsignaturaModel asignaturaModel);

    }
}
