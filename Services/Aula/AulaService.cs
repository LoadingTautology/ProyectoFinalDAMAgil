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
                NumeroAula = aulaModel.NumeroAula,
                NombreAula = aulaModel.NombreAula,
                AforoMax = aulaModel.AforoMax,
                IdCentro = aulaModel.IdCentro
            };
            _context.Add(aulaDB);
            _context.SaveChanges();
            return aulaModel;
        }

        public async Task<AulaModel> ReadAula(int NumeroAula, int IdCentro)
        {
            IQueryable<Scaffold.Aula> aulaListaDB = from aula in _context.Aulas
                                                    where aula.NumeroAula == NumeroAula && aula.IdCentro == IdCentro
                                                    select new Scaffold.Aula
                                                    {
                                                        IdAula = aula.IdAula,
                                                        NumeroAula = aula.NumeroAula,
                                                        NombreAula = aula.NombreAula,
                                                        AforoMax = aula.AforoMax,
                                                        IdCentro = aula.IdCentro
                                                    };

            Scaffold.Aula aulaDB = aulaListaDB.FirstOrDefault();
            
            AulaModel aulaModel = null;

            if (aulaDB != null)
            {
                aulaModel= new AulaModel()
                {
                    IdAula = aulaDB.IdAula,
                    NumeroAula = aulaDB.NumeroAula,
                    NombreAula = aulaDB.NombreAula,
                    AforoMax = aulaDB.AforoMax,
                    IdCentro = aulaDB.IdCentro
                };

            }
            return aulaModel;


        }

        public async Task<AulaModel> UpdateAula(AulaModel aulaModel)
        {
            Scaffold.Aula aula = new Scaffold.Aula()
            {
                IdAula = aulaModel.IdAula,
                NumeroAula = aulaModel.NumeroAula,
                NombreAula = aulaModel.NombreAula,
                IdCentro = aulaModel.IdCentro,
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
                NumeroAula = aulaModel.NumeroAula,
                NombreAula = aulaModel.NombreAula,
                AforoMax = aulaModel.AforoMax,
                IdCentro = aulaModel.IdCentro
            };

            _context.Aulas.Remove(aula);
            _context.SaveChangesAsync();         
            return aulaModel;
        }

        public async Task<IEnumerable<AulaModel>> ListadoAulas(int idCentro)
        {
            IQueryable<AulaModel> aulaListaDB = from aula in _context.Aulas
                                                where aula.IdCentro == idCentro
                                                select new AulaModel
                                                {
                                                    IdAula = aula.IdAula,
                                                    NumeroAula = aula.NumeroAula,
                                                    NombreAula = aula.NombreAula,
                                                    AforoMax = aula.AforoMax,
                                                    IdCentro = aula.IdCentro

                                                };

            return aulaListaDB.ToList();
        }

        public async Task<bool> ExistAula(AulaModel aulaModel)
        {
            bool existe = false;

            IQueryable<AulaModel> aulaListaDB
                    = from aula in _context.Aulas
                      where aula.NumeroAula == aulaModel.NumeroAula && aula.IdCentro == aulaModel.IdCentro
                      //where cicloformativo.IdCentro==cicloformativoModel.IdCentro &&
                      //  (cicloformativo.Acronimo== cicloformativoModel.Acronimo || cicloformativo.NombreCiclo == cicloformativoModel.NombreCiclo)
                      select new AulaModel
                      {
                          IdAula = aula.IdAula,
                          NombreAula = aula.NombreAula,
                          NumeroAula = aula.NumeroAula,
                          AforoMax = aula.AforoMax,
                          IdCentro = aula.IdCentro
                      };

            if (aulaListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<AulaModel> ReadAula(int idAula)
        {
            IQueryable<Scaffold.Aula> aulaListaDB = from aula in _context.Aulas
                                                    where aula.IdAula == idAula
                                                    select new Scaffold.Aula
                                                    {
                                                        IdAula = aula.IdAula,
                                                        NumeroAula = aula.NumeroAula,
                                                        NombreAula = aula.NombreAula,
                                                        AforoMax = aula.AforoMax,
                                                        IdCentro = aula.IdCentro
                                                    };

            Scaffold.Aula aulaDB = aulaListaDB.FirstOrDefault();

            AulaModel aulaModel = null;

            if (aulaDB != null)
            {
                aulaModel= new AulaModel()
                {
                    IdAula = aulaDB.IdAula,
                    NumeroAula = aulaDB.NumeroAula,
                    NombreAula = aulaDB.NombreAula,
                    AforoMax = aulaDB.AforoMax,
                    IdCentro = aulaDB.IdCentro
                };

            }
            return aulaModel;
        }
    }
}
