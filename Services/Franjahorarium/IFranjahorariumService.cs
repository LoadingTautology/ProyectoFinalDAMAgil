using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Franjahorarium
{
    public interface IFranjahorariumService
    {
        Task<IEnumerable<FranjahorariumModel>> ListFranjahorarium();
    }
}
