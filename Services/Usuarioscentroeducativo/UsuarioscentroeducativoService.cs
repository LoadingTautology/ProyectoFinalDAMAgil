
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo
{
    public class UsuarioscentroeducativoService : IUsuarioscentroeducativoService
    {
        private readonly DbappProyectoFinalContext _context;

        public UsuarioscentroeducativoService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<Scaffold.Usuarioscentroeducativo> GetUsuariosCentroeducativo(int idCentro, int idUsuario)
        {
            Scaffold.Usuarioscentroeducativo usuarioscentroeducativo = await _context.Usuarioscentroeducativos.Where(user => user.IdCentro == idCentro && user.IdUsuario == idUsuario).FirstOrDefaultAsync();
            return usuarioscentroeducativo;
        }

        public async Task<Scaffold.Usuarioscentroeducativo> SaveUsuariosCentroeducativo(Scaffold.Usuarioscentroeducativo usuarioCentroEducativo)
        {
            _context.Usuarioscentroeducativos.Add(usuarioCentroEducativo);
            await _context.SaveChangesAsync();
            Scaffold.Usuarioscentroeducativo usuarioCentroEducativobbdd = await GetUsuariosCentroeducativo(usuarioCentroEducativo.IdCentro, usuarioCentroEducativo.IdUsuario);

            return usuarioCentroEducativobbdd;
        }
    }
}
