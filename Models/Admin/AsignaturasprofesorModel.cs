namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class AsignaturasprofesorModel
    {
        /*Asignaturas*/
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public int Curso { get; set; }

        /*Estudios*/
        public int IdCiclo { get; set; }
        public string Acronimo { get; set; }

        /*Asignaturasprofesor*/
        public int IdProfesor { get; set; }

        /*Usuario*/
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }

        public override string ToString()
        {
            return $"AsignaturasprofesorModel: \n" +
                   $"  IdAsignatura: {IdAsignatura}\n" +
                   $"  NombreAsignatura: {NombreAsignatura}\n" +
                   $"  Curso: {Curso}\n" +
                   $"  IdCiclo: {IdCiclo}\n" +
                   $"  Acronimo: {Acronimo}\n" +
                   $"  IdProfesor: {IdProfesor}\n" +
                   $"  NombreUsuario: {NombreUsuario}\n" +
                   $"  ApellidosUsuario: {ApellidosUsuario}";
        }

    }
}
