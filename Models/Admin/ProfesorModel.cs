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

        public override string ToString()
        {
            return " IdProfesor: "+IdProfesor+" Especialidad: "+Especialidad+" IdCentro: "+IdCentro+
                   " NombreUsuario: "+NombreUsuario+" ApellidosUsuario: "+ ApellidosUsuario+" Email:"+Email;
        }

    }
}
