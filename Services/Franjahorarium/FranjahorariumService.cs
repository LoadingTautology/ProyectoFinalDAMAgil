using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Franjahorarium
{
    public class FranjahorariumService : IFranjahorariumService
    {
        private readonly DbappProyectoFinalContext _context;

        public FranjahorariumService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<IEnumerable<FranjahorariumModel>> ListFranjahorarium()
        {
            IQueryable<FranjahorariumModel> listaFranjaHoriasDB =  from franja in _context.Franjahoraria
                                                                   select new FranjahorariumModel
                                                                   {
                                                                       IdFranja = franja.IdFranja,
                                                                       HoraMinInicio = franja.HoraMinInicio,
                                                                       HoraMinFinal = franja.HoraMinFinal
                                                                   };

            return listaFranjaHoriasDB.ToList();
        }
    }
}
