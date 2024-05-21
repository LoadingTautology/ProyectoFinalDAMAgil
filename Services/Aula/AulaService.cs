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

        public async Task<AulaModel> CreateAula(AulaModel aulaModel)
        {
            Scaffold.Aula aulaDB = new Scaffold.Aula() 
            { 
                IdAula = aulaModel.IdAula,
                NombreAula = aulaModel.NombreAula,
                NumeroAula = aulaModel.NumeroAula,
                AforoMax = aulaModel.AforoMax
            };
            _context.Add(aulaDB);
            _context.SaveChanges();
            return aulaModel;
        }

        public async Task<AulaModel> ReadAula(int idAula)
        {
            IQueryable<Scaffold.Aula> aulaListaDB = from aula in _context.Aulas
                                                    where aula.IdAula == idAula
                                                    select new Scaffold.Aula
                                                    {
                                                        IdAula = aula.IdAula,
                                                        NombreAula = aula.NombreAula,
                                                        NumeroAula = aula.NumeroAula,
                                                        AforoMax = aula.AforoMax
                                                    };

            Scaffold.Aula aulaDB = aulaListaDB.FirstOrDefault();
            
            AulaModel aulaModel = null;

            if (aulaDB != null)
            {
                aulaModel= new AulaModel()
                {
                    IdAula = aulaDB.IdAula,
                    NombreAula = aulaDB.NombreAula,
                    NumeroAula = aulaDB.NumeroAula,
                    AforoMax = aulaDB.AforoMax
                };

            }
            return aulaModel;


        }

        public async Task<AulaModel> UpdateAula(AulaModel aulaModel)
        {
            Scaffold.Aula aula = new Scaffold.Aula()
            {
                IdAula = aulaModel.IdAula,
                NombreAula = aulaModel.NombreAula,
                NumeroAula = aulaModel.NumeroAula,
                AforoMax = aulaModel.AforoMax
            };
            _context.Aulas.Update(aula);
            _context.SaveChangesAsync();
            return aulaModel;
        }
        public async Task<AulaModel> DeleteAula(AulaModel aulaModel)
        {
            Scaffold.Aula aula = new Scaffold.Aula()
            {
                IdAula = aulaModel.IdAula,
                NombreAula = aulaModel.NombreAula,
                NumeroAula = aulaModel.NumeroAula,
                AforoMax = aulaModel.AforoMax
            };

            _context.Aulas.Remove(aula);
            _context.SaveChangesAsync();         
            return aulaModel;
        }

        public async Task<IEnumerable<AulaModel>> ListadoAula(int idAula)
        {
            IQueryable<AulaModel> aulaListaDB = from aula in _context.Aulas
                                                where aula.IdAula == idAula
                                                select new AulaModel
                                                {
                                                    IdAula = aula.IdAula,
                                                    NombreAula = aula.NombreAula,
                                                    NumeroAula = aula.NumeroAula,
                                                    AforoMax = aula.AforoMax
                                                };

            return aulaListaDB.ToList();
        }

        public async Task<bool> ExistAula(AulaModel aulaModel)
        {
            bool existe = false;

            IQueryable<AulaModel> aulaListaDB
                    = from aula in _context.Aulas
                      where aula.IdAula == aulaModel.IdAula
                      //where cicloformativo.IdCentro==cicloformativoModel.IdCentro &&
                      //  (cicloformativo.Acronimo== cicloformativoModel.Acronimo || cicloformativo.NombreCiclo == cicloformativoModel.NombreCiclo)
                      select new AulaModel
                      {
                          IdAula = aula.IdAula,
                          NombreAula = aula.NombreAula,
                          NumeroAula = aula.NumeroAula,
                          AforoMax = aula.AforoMax
                      };

            if (aulaListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

    }
}
