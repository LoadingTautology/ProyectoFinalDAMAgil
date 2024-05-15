
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Administrador
{
    public class AdministradorService : IAdministradorService
    {
        private readonly DbappProyectoFinalContext _context;

        public AdministradorService(DbappProyectoFinalContext context)
        {
            _context = context;
        }
        public async Task<Scaffold.Administrador> GetAdministrador(int IdAdministrador)
        {
            Scaffold.Administrador admin = await _context.Administradors.Where(user => user.IdAdministrador == IdAdministrador).FirstOrDefaultAsync();
            return admin;
        }

        public async Task<Scaffold.Administrador> SaveAdministrador(Scaffold.Administrador admin)
        {
            _context.Administradors.Add(admin);
            await _context.SaveChangesAsync();
            Scaffold.Administrador adminbbdd = await GetAdministrador(admin.IdAdministrador);

            return adminbbdd;
        }
    }
}
