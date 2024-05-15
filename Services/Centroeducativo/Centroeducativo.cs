
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Centroeducativo
{
    public class Centroeducativo : ICentroeducativo
    {
        //Inyecta la base de datos para poder implementar los metodos de la interfaz
        private readonly DbappProyectoFinalContext _context;

        public Centroeducativo(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<Scaffold.Centroeducativo> GetCentroeducativo(string nombreCentro, string direccion)
        {
            Scaffold.Centroeducativo centroeducativo = await _context.Centroeducativos.Where(centro => centro.NombreCentro == nombreCentro && centro.Direccion == direccion).FirstOrDefaultAsync();

            return centroeducativo;
        }

        public async Task<Scaffold.Centroeducativo> SaveCentroeducativo(Scaffold.Centroeducativo centroEducativo)
        {
            _context.Centroeducativos.Add(centroEducativo);
            await _context.SaveChangesAsync();
            Scaffold.Centroeducativo centroeducativobbdd = await GetCentroeducativo(centroEducativo.NombreCentro, centroEducativo.Direccion);
            return centroeducativobbdd;
        }
    }
}
