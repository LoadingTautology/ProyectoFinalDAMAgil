using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using System.Linq;

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

        public async Task<Scaffold.Centroeducativo> GetCentroeducativo(int idCentro)
        {
            Scaffold.Centroeducativo centroeducativo = await _context.Centroeducativos.Where(centro => centro.IdCentro == idCentro).FirstOrDefaultAsync();

            return centroeducativo!;
        }


        public async Task<Scaffold.Centroeducativo> GetCentroeducativo(string nombreCentro, string direccion)
        {
            Scaffold.Centroeducativo centroeducativo = await _context.Centroeducativos.Where(centro => centro.NombreCentro == nombreCentro && centro.Direccion == direccion).FirstOrDefaultAsync();

            return centroeducativo!;
        }

        public async Task<Scaffold.Centroeducativo> SaveCentroeducativo(Scaffold.Centroeducativo centroEducativo, string emailAdmin)
        {
            Scaffold.Usuario userAdmin = await _context.Usuarios.Where(user => user.Email == emailAdmin).FirstOrDefaultAsync();
            centroEducativo.IdAdministrador = userAdmin!.IdUsuario;
            _context.Centroeducativos.Add(centroEducativo);
            await _context.SaveChangesAsync();
            Scaffold.Centroeducativo centroeducativobbdd = await GetCentroeducativo(centroEducativo.NombreCentro, centroEducativo.Direccion);
            return centroeducativobbdd;
        }

        public async Task<bool> ExisteCentroEducativo(string nombreCentro, string direccion)
        {
            bool existe = false;

            Scaffold.Centroeducativo centrobbdd = _context.Centroeducativos.Where(centro => centro.NombreCentro == nombreCentro && centro.Direccion==direccion).FirstOrDefault()!;
            if (centrobbdd!=null)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<Scaffold.Centroeducativo>> ListadoCentroEducativo(string emailAdmin)
        {
            IQueryable<Scaffold.Centroeducativo> centrobbdd = from centro in _context.Centroeducativos
                                                              join user in _context.Usuarios on centro.IdAdministrador equals user.IdUsuario
                                                              where user.Email == emailAdmin
                                                              select new Scaffold.Centroeducativo { IdCentro=centro.IdCentro, NombreCentro=centro.NombreCentro, Direccion=centro.Direccion };

            //List<Scaffold.Centroeducativo> centroView = new List<Scaffold.Centroeducativo>();

            //foreach( var item in centrobbdd) 
            //{
            //    centroView.Add(new Scaffold.Centroeducativo { IdCentro=item.IdCentro, NombreCentro=item.NombreCentro, Direccion=item.Direccion } );
            //}


            return centrobbdd.ToList();
        }





        //public async Task<IEnumerable<CentroEducativoModel>> ListadoCentroEducativo(string emailAdmin)
        //{



        //    IQueryable<CentroEducativoModel> centrobbdd = from centro in _context.Centroeducativos
        //                                                  join user in _context.Usuarios on centro.IdAdministrador equals user.IdUsuario
        //                                                  where user.Email == emailAdmin
        //                                                  select new CentroEducativoModel { NombreCentro=centro.NombreCentro, DireccionCentro= centro.Direccion };
        //    return centrobbdd.ToList();

        //}
    }
}
