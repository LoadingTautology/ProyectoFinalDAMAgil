namespace ProyectoFinalDAMAgil.Services.Administrador
{
    public interface IAdministradorService
    {
        //Sirve para autenticar un usuario que se extrae de la bbdd
        Task<Scaffold.Administrador> GetAdministrador(int IdAdministrador);

        //Sirve guardar un usuario en la bbdd
        Task<Scaffold.Administrador> SaveAdministrador(Scaffold.Administrador Admin);

    }
}
