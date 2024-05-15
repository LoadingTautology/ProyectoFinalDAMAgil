namespace ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo
{
    public interface IUsuarioscentroeducativo
    {
        Task<Scaffold.Usuarioscentroeducativo> GetUsuariosCentroeducativo(int IdCentro, int IdUsuario);

        //Sirve guardar un usuario en la bbdd
        Task<Scaffold.Usuarioscentroeducativo> SaveUsuariosCentroeducativo(Scaffold.Usuarioscentroeducativo UsuarioCentroEducativo);

    }
}
