namespace ProyectoFinalDAMAgil.Services.Usuario
{
	public interface IUsuarioService
	{
		//Sirve para autenticar un usuario que se extrae de la bbdd
		Task<Scaffold.Usuario> GetUsuario(string correo);

		//Sirve guardar un usuario en la bbdd
		Task<Scaffold.Usuario> SaveUsuario(Scaffold.Usuario usuario);

	}
}
