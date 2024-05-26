using ProyectoFinalDAMAgil.Models.Admin;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Services.Cicloformativo
{
    public class CicloformativoService : ICicloformativoService
    {
        private readonly DbappProyectoFinalContext _context;

        public CicloformativoService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<CicloformativoModel> CreateCicloformativo(CicloformativoModel cicloformativoModel)
        {
            Scaffold.Cicloformativo cicloformativo = new Scaffold.Cicloformativo
            {
                NombreCiclo=cicloformativoModel.NombreCiclo,
                Acronimo=cicloformativoModel.Acronimo,
                IdCentro=cicloformativoModel.IdCentro
            };
            _context.Add(cicloformativo);
            _context.SaveChanges();

            return cicloformativoModel;
        }

        public async Task<CicloformativoModel> ReadCicloformativo(int idEstudio)
        {
            IQueryable<CicloformativoModel> cicloformativoListaDB = from cicloformativo in _context.Cicloformativos 
                                                                        where cicloformativo.IdCiclo == idEstudio
                                                                        select new CicloformativoModel
                                                                        {
                                                                            IdCiclo=cicloformativo.IdCiclo,
                                                                            NombreCiclo=cicloformativo.NombreCiclo,
                                                                            Acronimo=cicloformativo.Acronimo,
                                                                            IdCentro=cicloformativo.IdCentro
                                                                        };

            return cicloformativoListaDB.FirstOrDefault();

        }

        public async Task<CicloformativoModel> UpdateCicloformativo(CicloformativoModel cicloformativoModel)
        {
            Scaffold.Cicloformativo cicloformativo = new Scaffold.Cicloformativo
            {
                IdCiclo=cicloformativoModel.IdCiclo,
                NombreCiclo=cicloformativoModel.NombreCiclo,
                Acronimo=cicloformativoModel.Acronimo,
                IdCentro=cicloformativoModel.IdCentro
            };
            _context.Cicloformativos.Update(cicloformativo);
            await _context.SaveChangesAsync();
            return cicloformativoModel;
        }

        public async Task<CicloformativoModel> DeleteCicloformativo(CicloformativoModel cicloformativoModel)
        {

            Scaffold.Cicloformativo cicloformativo = new Scaffold.Cicloformativo
            {
                IdCiclo=cicloformativoModel.IdCiclo,
                NombreCiclo=cicloformativoModel.NombreCiclo,
                Acronimo=cicloformativoModel.Acronimo,
                IdCentro=cicloformativoModel.IdCentro
            };
            _context.Cicloformativos.Remove(cicloformativo);
            await _context.SaveChangesAsync();
            return cicloformativoModel;
        }

        public async Task<IEnumerable<CicloformativoModel>> ListadoCicloformativo(int idCentro)
        {
            IQueryable<CicloformativoModel> cicloformativoListaDB = from cicloformativo in _context.Cicloformativos
                                                                    where cicloformativo.IdCentro == idCentro
                                                                    select new CicloformativoModel
                                                                        {
                                                                            IdCiclo=cicloformativo.IdCiclo,
                                                                            NombreCiclo=cicloformativo.NombreCiclo,
                                                                            Acronimo=cicloformativo.Acronimo,
                                                                            IdCentro=idCentro
                                                                        };

            return cicloformativoListaDB.ToList();
        }

        public async Task<bool> ExistCicloformativo(CicloformativoModel cicloformativoModel)
        {           

            bool existe = false;

            IQueryable<CicloformativoModel> cicloformativoListaDB 
                    = from cicloformativo in _context.Cicloformativos
                      where cicloformativo.IdCentro==cicloformativoModel.IdCentro && 
                        (cicloformativo.Acronimo== cicloformativoModel.Acronimo || cicloformativo.NombreCiclo == cicloformativoModel.NombreCiclo)
                      select new CicloformativoModel
                      {
                          IdCiclo=cicloformativo.IdCiclo,
                          NombreCiclo=cicloformativo.NombreCiclo,
                          Acronimo=cicloformativo.Acronimo,
                          IdCentro=cicloformativoModel.IdCentro
                      };

            if (cicloformativoListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;

        }

    }
}

