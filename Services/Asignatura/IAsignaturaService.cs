using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Asignatura
{
    public interface IAsignaturaService
    {
        Task<AsignaturaModel> CreateAsignatura(AsignaturaModel asignaturaModel, int idCiclo);
        Task<AsignaturaModel> ReadAsignatura(int idAsignatura);
        Task<AsignaturaModel> UpdateAsignatura(AsignaturaModel asignaturaModel);
        Task<AsignaturaModel> DeleteAsignatura(AsignaturaModel asignaturaModel, int idEstudios);

        Task<IEnumerable<AsignaturaModel>> ListadoAsignatura(int idCiclo);
        Task<IEnumerable<AsignaturaModel>> ListadoAsignaturaCentro(int idCentro);
        Task<bool> ExistAsignatura(AsignaturaModel asignaturaModel, int idCiclo, int idCentro);
        Task<IEnumerable<CicloformativoModel>> ListadoCiclos(AsignaturaModel asignaturaModel);

        Task<AsignaturaModel> VincularAsignaturaCiclo(AsignaturaModel asignaturaModel, int idCiclo);
        Task<AsignaturaModel> DesvincularAsignaturaCiclo(AsignaturaModel asignaturaModel, int idCiclo);

    }
}
