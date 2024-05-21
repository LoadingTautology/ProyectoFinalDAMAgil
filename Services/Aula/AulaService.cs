using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Aula
{
    public class AulaService : IAulaService
    {
        private readonly DbappProyectoFinalContext _context;

        public AulaService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public Task<AulaModel> CreateAula(AulaModel aulaModel)
        {
            throw new NotImplementedException();
        }

        public Task<AulaModel> DeleteAula(AulaModel aulaModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAula(AulaModel aulaModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AulaModel>> ListadoAula(int idAula)
        {
            throw new NotImplementedException();
        }

        public Task<AulaModel> ReadAula(int idAula)
        {
            throw new NotImplementedException();
        }

        public Task<AulaModel> UpdateAula(AulaModel aulaModel)
        {
            throw new NotImplementedException();
        }
    }
}
