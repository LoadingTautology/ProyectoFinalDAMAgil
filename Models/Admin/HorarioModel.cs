namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class HorarioModel
    {
        public int IdAula { get; set; }

        public int IdDiaSemanaFranjaHoraria { get; set; }

        public string ColorAsignatura { get; set; }

        public int IdAsignatura { get; set; }

        public int IdEstudios { get; set; }

        //public int IdAsignaturasEstudios { get; set; }


        public override string ToString()
        {
            return "IdAula: "+IdAula+" IdDiaFranja: "+IdDiaSemanaFranjaHoraria+" ColorAsignatura: "+ColorAsignatura+" IdAsignatura: "+IdAsignatura+" IdEstudios: "+IdEstudios;
            //return "IdAula: "+IdAula+" IdDiaFranja: "+IdDiaSemanaFranjaHoraria+" ColorAsignatura: "+ColorAsignatura+" IdAsignaturasEstudios: "+IdAsignaturasEstudios; 
        }

    }
}
