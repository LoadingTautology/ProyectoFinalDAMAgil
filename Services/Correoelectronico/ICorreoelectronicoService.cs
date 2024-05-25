using ProyectoFinalDAMAgil.Models.Admin;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProyectoFinalDAMAgil.Services.Correoelectronico
{
	public interface ICorreoelectronicoService
	{
		Task<Scaffold.Correoelectronico> GetCorreoElectronico(string email, string clave);

		//Sirve guardar un usuario en la bbdd
		Task<Scaffold.Correoelectronico> SaveCorreoElectronico(Scaffold.Correoelectronico correo);

		Task<bool> ExistCorreoElectronico(string email);

        Task<Scaffold.Correoelectronico> DeleteCorreoElectronico(string email);

        Task<Scaffold.Correoelectronico> ReadCorreoElectronico(string email);


    }
}
