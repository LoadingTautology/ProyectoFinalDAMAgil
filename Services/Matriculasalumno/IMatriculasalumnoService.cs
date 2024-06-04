using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Matriculasalumno
{
    public interface IMatriculasalumnoService
    {
        Task<MatriculasalumnoModel> CreateMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura);

        Task<MatriculasalumnoModel> ReadMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura);

        Task<MatriculasalumnoModel> UpdateMatriculasalumno(MatriculasalumnoModel matriculasalumnoModel);

        Task<MatriculasalumnoModel> DeleteMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura);

        Task<bool> ExistMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura);

        Task<IEnumerable<MatriculasalumnoModel>> ListAsignaturasMatriculadasAlumno(int idAlumno);

        Task<IEnumerable<HorarioModel>> ListHorariosAlumno(int idAlumno);

        Task<IEnumerable<AsignaturaModel>> ListAsignaturasAlumno(int idAlumno);

        Task<IEnumerable<CicloformativoModel>> ListEstudiosAlumno(int idAlumno);

        Task<IEnumerable<AulaModel>> ListAulasAlumno(int idAlumno);

        Task<bool> ExistHorarioEnConflictoAlumno(int idAlumno, int idEstudio, int idAsignatura);

        Task<IEnumerable<MatriculasalumnoModel>> ListAlumnosMatriculadosAsignaturaEstudio(int idEstudio, int idAsignatura);
    }
}
