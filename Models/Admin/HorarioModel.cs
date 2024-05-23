namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class HorarioModel
    {
        public int IdHorario { get; set; }

        public int IdAula { get; set; }

        public int IdDiaFranja { get; set; }

        public int IdAsignatura { get; set; }

        public int IdEstudio { get; set; }

        public string ColorAsignatura { get; set; } = null!;

        public override string ToString()
        {
            return "IdAula: "+IdAula+" IdDiaFranja: "+IdDiaFranja+" ColorAsignatura: "+ColorAsignatura+" IdAsignatura: "+IdAsignatura+" IdEstudios: "+IdEstudio;
        }

    }
}
