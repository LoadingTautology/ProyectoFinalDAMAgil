namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class AlumnoModel
    {
        public int IdAlumno { get; set; }

        public DateOnly FechaDeNacimiento { get; set; }

        public int IdCentro { get; set; }

        /* ************************************************** */

        public string NombreUsuario { get; set; }

        public string ApellidosUsuario { get; set; }

        public string Email { get; set; }

    }
}
