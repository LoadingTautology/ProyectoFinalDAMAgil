using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProyectoFinalDAMAgil.Services.Correoelectronico
{
	public interface ICorreoelectronicoService
	{
		Task<Scaffold.Correoelectronico> GetCorreoElectronico(string email, string clave);

		//Sirve guardar un usuario en la bbdd
		Task<Scaffold.Correoelectronico> SaveCorreoElectronico(Scaffold.Correoelectronico correo);

	}
}
