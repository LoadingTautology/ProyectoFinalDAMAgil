using ProyectoFinalDAMAgil.Models.Admin;

namespace ProyectoFinalDAMAgil.Services.Cicloformativo
{
    public interface ICicloformativoService
    {
        Task<CicloformativoModel> CreateCicloformativo(CicloformativoModel cicloformativoModel);
        Task<CicloformativoModel> ReadCicloformativo(int idCiclo);
        Task<CicloformativoModel> UpdateCicloformativo(CicloformativoModel cicloformativoModel);
        Task<CicloformativoModel> DeleteCicloformativo(CicloformativoModel cicloformativoModel);
        
        Task<IEnumerable<CicloformativoModel>> ListadoCicloformativo(int idCiclo);


    }
}
