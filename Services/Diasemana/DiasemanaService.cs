using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Diasemana
{
    public class DiasemanaService : IDiasemanaService
    {
        private readonly DbappProyectoFinalContext _context;

        public DiasemanaService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<IEnumerable<DiasemanaModel>> ListDiasemana()
        {
            IQueryable<DiasemanaModel> listaDiasSemanaDB = from dia in _context.Diasemanas
                                                         select new DiasemanaModel
                                                         {
                                                             IdDia = dia.IdDia,
                                                             DiaDeLaSemana =  dia.DiaDeLaSemana
                                                         };


            return listaDiasSemanaDB.ToList();
        }
    }
}
