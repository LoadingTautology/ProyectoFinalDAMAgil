using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Aula
{
    public interface IAulaService
    {
        Task<AulaModel> CreateAula(AulaModel aulaModel);
        Task<AulaModel> ReadAula(int idAula);
        Task<AulaModel> UpdateAula(AulaModel aulaModel);
        Task<AulaModel> DeleteAula(AulaModel aulaModel);

        Task<IEnumerable<AulaModel>> ListadoAula(int idAula);
        Task<bool> ExistAula(AulaModel aulaModel);

    }
}
