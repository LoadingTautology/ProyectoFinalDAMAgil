using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Alumno
{
    public interface IAlumnoService
    {
        Task<AlumnoModel> CreateAlumno(AlumnoModel alumnoModel, string password);
        Task<AlumnoModel> ReadAlumno(int idAlumno);
        Task<AlumnoModel> UpdateAlumno(AlumnoModel alumnoModel);
        Task<AlumnoModel> DeleteAlumno(AlumnoModel alumnoModel);

        Task<IEnumerable<AlumnoModel>> ListadoAlumnos(int idCentro);

        Task<bool> ExistAlumno(string emailAlumno, int idCentro);

        Task<IEnumerable<AlumnoModel>> ListadoAlumnos(int idEstudio, int idAsignatura);
    }
}
