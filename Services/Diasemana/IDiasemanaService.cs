using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Diasemana
{
    public interface IDiasemanaService
    {
        Task<IEnumerable<DiasemanaModel>> ListDiasemana();

    }
}
