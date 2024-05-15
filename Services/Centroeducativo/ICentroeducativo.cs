namespace ProyectoFinalDAMAgil.Services.Centroeducativo
{
    public interface ICentroeducativo
    {
        Task<Scaffold.Centroeducativo> GetCentroeducativo(string NombreCentro, string Direccion);

        //Sirve guardar un usuario en la bbdd
        Task<Scaffold.Centroeducativo> SaveCentroeducativo(Scaffold.Centroeducativo CentroEducativo);
    }
}
