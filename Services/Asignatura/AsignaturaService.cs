using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using System.Security.Cryptography.Xml;

namespace ProyectoFinalDAMAgil.Services.Asignatura
{
    public class AsignaturaService : IAsignaturaService
    {
        private readonly DbappProyectoFinalContext _context;

        public AsignaturaService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<AsignaturaModel> CreateAsignatura(AsignaturaModel asignaturaModel,int idCiclo)
        {
            Scaffold.Asignatura asignatura = new Scaffold.Asignatura()
            {
                NombreAsignatura = asignaturaModel.NombreAsignatura,
                Curso = asignaturaModel.Curso
            };

            _context.Add(asignatura);
            _context.SaveChanges();

            IQueryable<AsignaturaModel> asignaturaListaDB = from asig in _context.Asignaturas
                                                                where asig.NombreAsignatura == asignaturaModel.NombreAsignatura && asig.Curso == asignaturaModel.Curso
                                                                select new AsignaturaModel()
                                                                {
                                                                    IdAsignatura = asignatura.IdAsignatura,
                                                                    NombreAsignatura = asignatura.NombreAsignatura,
                                                                    Curso =asignatura.Curso
                                                                };

            AsignaturaModel asignaturaDB = asignaturaListaDB.FirstOrDefault();

            this.VincularAsignaturaCiclo(asignaturaDB, idCiclo);

            return asignaturaModel;
        }

        public async Task<AsignaturaModel> ReadAsignatura(int idAsignatura)
        {
            IQueryable<Scaffold.Asignatura> asignaturaListaDB = from asignatura in _context.Asignaturas
                                                                where asignatura.IdAsignatura == idAsignatura
                                                                select new Scaffold.Asignatura()
                                                                {
                                                                    IdAsignatura = asignatura.IdAsignatura,
                                                                    NombreAsignatura = asignatura.NombreAsignatura,
                                                                    Curso =asignatura.Curso
                                                                };

            Scaffold.Asignatura asignaturaDB = asignaturaListaDB.FirstOrDefault();
            AsignaturaModel asignaturaModel = null;

            if (asignaturaDB != null)
            {
                asignaturaModel= new AsignaturaModel()
                {
                    IdAsignatura = asignaturaDB.IdAsignatura,
                    NombreAsignatura = asignaturaDB.NombreAsignatura,
                    Curso =asignaturaDB.Curso
                };

            }
            return asignaturaModel;


        }

        public async Task<AsignaturaModel> UpdateAsignatura(AsignaturaModel asignaturaModel)
        {
            Scaffold.Asignatura asignatura = new Scaffold.Asignatura()
            {
                IdAsignatura = asignaturaModel.IdAsignatura,
                NombreAsignatura = asignaturaModel.NombreAsignatura,
                Curso =asignaturaModel.Curso
            };

            _context.Asignaturas.Update(asignatura);
            _context.SaveChanges();
            return asignaturaModel;
        }

        public async Task<AsignaturaModel> DeleteAsignatura(AsignaturaModel asignaturaModel, int idEstudios)
        {
            this.DesvincularAsignaturaCiclo(asignaturaModel, idEstudios);


            IQueryable<AsignaturaModel> asignaturaListaDB
                = from asignatura in _context.Asignaturas
                  join asignaturaciclo in _context.Asignaturascicloformativos on asignatura.IdAsignatura equals asignaturaciclo.IdAsignatura                
                  where asignaturaciclo.IdAsignatura== asignaturaModel.IdAsignatura
                  select new AsignaturaModel()
                  {
                      IdAsignatura = asignatura.IdAsignatura,
                      NombreAsignatura = asignatura.NombreAsignatura,
                      Curso =asignatura.Curso
                  };


            if (asignaturaListaDB.Count()==0)
            {
                Scaffold.Asignatura asignaturaDB = new Scaffold.Asignatura()
                {
                    IdAsignatura = asignaturaModel.IdAsignatura,
                    NombreAsignatura = asignaturaModel.NombreAsignatura,
                    Curso =asignaturaModel.Curso
                };
                _context.Asignaturas.Remove(asignaturaDB);
                _context.SaveChanges();
            }

            return asignaturaModel;

        }

        public async Task<IEnumerable<AsignaturaModel>> ListadoAsignatura(int idCiclo)
        {
            IQueryable<AsignaturaModel> asignaturaListaDB 
                = from asignatura in _context.Asignaturas
                    join asignaturaciclo in _context.Asignaturascicloformativos on asignatura.IdAsignatura equals asignaturaciclo.IdAsignatura
                  where asignaturaciclo.IdCiclo == idCiclo
                  select new AsignaturaModel()
                  {
                      IdAsignatura = asignatura.IdAsignatura,
                      NombreAsignatura = asignatura.NombreAsignatura,
                      Curso =asignatura.Curso
                  };

            return asignaturaListaDB.ToList();
        }

        public async Task<bool> ExistAsignatura(AsignaturaModel asignaturaModel,int idCiclo, int idCentro)
        {
            bool existe = false;

            IQueryable<AsignaturaModel> asignaturaListaDB
                = from asignatura in _context.Asignaturas
                  join asignaturaciclo in _context.Asignaturascicloformativos on asignatura.IdAsignatura equals asignaturaciclo.IdAsignatura
                  join cicloformativo in _context.Cicloformativos on asignaturaciclo.IdCiclo equals cicloformativo.IdCiclo
                  where  asignatura.NombreAsignatura==asignaturaModel.NombreAsignatura && asignatura.Curso==asignaturaModel.Curso 
                         && cicloformativo.IdCiclo == idCiclo && cicloformativo.IdCentro == idCentro
                  select new AsignaturaModel()
                  {
                      IdAsignatura = asignatura.IdAsignatura,
                      NombreAsignatura = asignatura.NombreAsignatura,
                      Curso =asignatura.Curso
                  };

            if (asignaturaListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<CicloformativoModel>> ListadoCiclos(AsignaturaModel asignaturaModel)
        {
            IQueryable<CicloformativoModel> cicloformativoListaDB
                = from asignatura in _context.Asignaturas
                    join asignaturaciclo in _context.Asignaturascicloformativos on asignatura.IdAsignatura equals asignaturaciclo.IdAsignatura
                    join ciclo in _context.Cicloformativos on asignaturaciclo.IdCiclo equals ciclo.IdCiclo
                    where asignatura.IdAsignatura == asignaturaModel.IdAsignatura
                    select new CicloformativoModel
                    {
                        IdCiclo=ciclo.IdCiclo,
                        NombreCiclo=ciclo.NombreCiclo,
                        Acronimo=ciclo.Acronimo,
                        IdCentro = ciclo.IdCentro
                    };

            return cicloformativoListaDB.ToList();
            
        }

        public async Task<IEnumerable<AsignaturaModel>> ListadoAsignaturaCentro(int idCentro)
        {
            IQueryable<AsignaturaModel> asignaturaListaDB
                = from asignatura in _context.Asignaturas
                  join asignaturaciclo in _context.Asignaturascicloformativos on asignatura.IdAsignatura equals asignaturaciclo.IdAsignatura
                  join cicloformativo in _context.Cicloformativos on asignaturaciclo.IdCiclo equals cicloformativo.IdCiclo
                  where cicloformativo.IdCentro == idCentro
                  select new AsignaturaModel()
                  {
                      IdAsignatura = asignatura.IdAsignatura,
                      NombreAsignatura = asignatura.NombreAsignatura,
                      Curso =asignatura.Curso
                  };

            return asignaturaListaDB.ToList();
        }

        public async Task<AsignaturaModel> VincularAsignaturaCiclo(AsignaturaModel asignaturaModel, int idCiclo)
        {

            Scaffold.Asignaturascicloformativo asignaturascicloformativo = new Scaffold.Asignaturascicloformativo()
            {
                IdAsignatura = asignaturaModel.IdAsignatura,
                IdCiclo = idCiclo
            };
            _context.Add(asignaturascicloformativo);
            _context.SaveChanges();

            return asignaturaModel;
        }

        public async Task<AsignaturaModel> DesvincularAsignaturaCiclo(AsignaturaModel asignaturaModel, int idCiclo)
        {
            IQueryable<Scaffold.Asignaturascicloformativo> asignaturaCicloListaDB
               = from asignaturaciclo in _context.Asignaturascicloformativos
                 where asignaturaciclo.IdAsignatura == asignaturaModel.IdAsignatura && asignaturaciclo.IdCiclo == idCiclo
                 select new Scaffold.Asignaturascicloformativo()
                 {
                     IdAsignaturasCicloFormativo = asignaturaciclo.IdAsignaturasCicloFormativo,
                     IdAsignatura = asignaturaciclo.IdAsignatura,
                     IdCiclo = asignaturaciclo.IdCiclo
                 };

            Scaffold.Asignaturascicloformativo asignaturaCicloDB = asignaturaCicloListaDB.FirstOrDefault();
            _context.Remove(asignaturaCicloDB);
            _context.SaveChanges();

            return asignaturaModel;
        }
    }
}

