using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Centroeducativo
{
    public interface ICentroeducativo
    {
        Task<Scaffold.Centroeducativo> GetCentroeducativo(int idCentro);

        Task<Scaffold.Centroeducativo> GetCentroeducativo(string nombreCentro, string direccion);

        Task<Scaffold.Centroeducativo> SaveCentroeducativo(Scaffold.Centroeducativo centroEducativo, string emailAdmin);

        Task<bool> ExisteCentroEducativo(string nombreCentro, string direccion);

        Task<IEnumerable<Scaffold.Centroeducativo>> ListadoCentroEducativo(string emailAdmin);
        //Task<IEnumerable<CentroEducativoModel>> ListadoCentroEducativo(string emailAdmin);
    }
}
