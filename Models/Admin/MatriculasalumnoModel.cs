namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class MatriculasalumnoModel
    {
        /*Asignaturas*/
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public int Curso { get; set; }

        /*Estudios*/
        public int IdCiclo { get; set; }
        public string Acronimo { get; set; }

        /*Matriculasalumno*/
        public int IdAlumno { get; set; }
        public float Eva1 { get; set; }
        public float Eva2 { get; set; }
        public float Eva3 { get; set; }

        /*Usuario*/
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }


    }
}
