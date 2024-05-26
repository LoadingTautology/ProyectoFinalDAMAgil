using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Aula
{
    public interface IAulaService
    {
        Task<AulaModel> CreateAula(AulaModel aulaModel);
        Task<AulaModel> ReadAula(int NumeroAula, int IdCentro);
        Task<AulaModel> UpdateAula(AulaModel aulaModel);
        Task<AulaModel> DeleteAula(AulaModel aulaModel);

        Task<IEnumerable<AulaModel>> ListadoAulas(int idCentro);
        Task<bool> ExistAula(AulaModel aulaModel);
        Task<AulaModel> ReadAula(int idAula);

        Task<IEnumerable<AulaModel>> ListadoAulasEstudio(int idEstudio);

    }
}
