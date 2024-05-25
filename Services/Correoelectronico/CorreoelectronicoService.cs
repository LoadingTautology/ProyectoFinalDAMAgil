
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Models.Admin;
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
            _context.SaveChanges();
            return correo;
        }

        public async Task<bool> ExistCorreoElectronico(string email)
        {
            bool existe = false;

            IQueryable<Scaffold.Correoelectronico> correoelectronicoListaDB = from correo in _context.Correoelectronicos
                                                                              where correo.Email == email
                                                                              select correo;

            if (correoelectronicoListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<Scaffold.Correoelectronico> DeleteCorreoElectronico(string email)
        {
            IQueryable<Scaffold.Correoelectronico> correoelectronicoListaDB = from correo in _context.Correoelectronicos
                                                                              where correo.Email == email
                                                                              select correo;
            
            Scaffold.Correoelectronico correoelectronico = correoelectronicoListaDB.FirstOrDefault();
            _context.Correoelectronicos.Remove(correoelectronico);
            _context.SaveChanges();

            return correoelectronico;

        }

        public async Task<Scaffold.Correoelectronico> ReadCorreoElectronico(string email)
        {
            IQueryable<Scaffold.Correoelectronico> correoelectronicoListaDB = from correo in _context.Correoelectronicos
                                                                              where correo.Email == email
                                                                              select correo;
            return correoelectronicoListaDB.FirstOrDefault();
        }
    }
}
