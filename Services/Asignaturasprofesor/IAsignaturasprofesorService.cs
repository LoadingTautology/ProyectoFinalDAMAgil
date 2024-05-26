using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Asignaturasprofesor
{
    public interface IAsignaturasprofesorService
    {
        Task<AsignaturasprofesorModel> CreateAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura);

        Task<AsignaturasprofesorModel> ReadAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura);

        Task<AsignaturasprofesorModel> DeleteAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura);

        Task<bool> ExistAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura);

        Task<IEnumerable<AsignaturasprofesorModel>> ListAsignaturasprofesor(int idProfesor);

        Task<IEnumerable<HorarioModel>> ListHorariosProfesor(int idProfesor);

        Task<IEnumerable<AsignaturaModel>> ListAsignaturasProfesor(int idProfesor);
        Task<IEnumerable<CicloformativoModel>> ListEstudiosProfesor(int idProfesor);

        Task<IEnumerable<AulaModel>> ListAulasProfesor(int idProfesor);

        Task<bool> ExistHorarioEnConflictoProfesor(int idProfesor, int idEstudio, int idAsignatura);


    }
}
