namespace ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo
{
    public interface IUsuarioscentroeducativoService
    {
        Task<Scaffold.Usuarioscentroeducativo> GetUsuariosCentroeducativo(int IdCentro, int IdUsuario);

        //Sirve guardar un usuario en la bbdd
        Task<Scaffold.Usuarioscentroeducativo> SaveUsuariosCentroeducativo(Scaffold.Usuarioscentroeducativo UsuarioCentroEducativo);

    }
}
