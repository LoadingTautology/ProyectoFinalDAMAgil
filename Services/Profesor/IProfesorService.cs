using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Profesor
{
    public interface IProfesorService
    {
        Task<ProfesorModel> CreateProfesor(ProfesorModel profesorModel, string password);
        Task<ProfesorModel> ReadProfesor(int idProfesor);
        Task<ProfesorModel> UpdateProfesor(ProfesorModel profesorModel);
        Task<ProfesorModel> DeleteProfesor(ProfesorModel profesorModel);

        Task<IEnumerable<ProfesorModel>> ListadoProfesores(int idCentro);

        Task<bool> ExistProfesor(string emailProfesor, int idCentro);

    }
}
