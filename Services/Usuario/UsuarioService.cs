﻿using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Scaffold;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProyectoFinalDAMAgil.Services.Usuario
{
	public class UsuarioService : IUsuarioService
	{
		//Inyecta la base de datos para poder implementar los metodos de la interfaz
		private readonly DbappProyectoFinalContext _context;


		public UsuarioService(DbappProyectoFinalContext context)
		{
			_context = context;
		}
		//Nos conectamos a la base de datos y le pedimos que traiga un usuario que cumpla con las especificaciones de "correo" y "clave"
		public async Task<Scaffold.Usuario> GetUsuario(string correo)
		{
			Scaffold.Usuario usuario = await _context.Usuarios.Where(user => user.Email == correo).FirstOrDefaultAsync();

			return usuario;
		}

		//Creamos un usuario y lo introducimos en la base de datos.
		public async Task<Scaffold.Usuario> SaveUsuario(Scaffold.Usuario usuario)
		{

			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();
			return usuario;
		}
	}
}
