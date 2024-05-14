
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Correoelectronico
{
	public class CorreoelectronicoService : ICorreoelectronicoService
	{
        private readonly DbappProyectoFinalContext _context;

        public CorreoelectronicoService(DbappProyectoFinalContext context)
        {
            _context = context;
        }

        public async Task<Scaffold.Correoelectronico> GetCorreoElectronico(string email, string clave)
		{
			Scaffold.Correoelectronico correoelectronico = await _context.Correoelectronicos.Where(user => user.Email == email && user.Clave == clave).FirstOrDefaultAsync();

			return correoelectronico;
		}

		public async Task<Scaffold.Correoelectronico> SaveCorreoElectronico(Scaffold.Correoelectronico correo)
		{
            _context.Correoelectronicos.Add(correo);
            await _context.SaveChangesAsync();
            return correo;
        }
	}
}
