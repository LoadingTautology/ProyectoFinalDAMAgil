namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class ProfesorModel
    {
        public int IdProfesor { get; set; }

        public string Especialidad { get; set; } = null!;

        public int IdCentro { get; set; }

        /* ************************************************** */

        public string NombreUsuario { get; set; }

        public string ApellidosUsuario { get; set; }

        public string Email { get; set; }

    }
}
