namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class CicloformativoModel
    {

        public int IdCiclo { get; set; }

        public string NombreCiclo { get; set; }

        public string Acronimo { get; set; } 

        public int IdCentro { get; set; }

        public override string ToString()
        {
            return "IdCiclo: "+IdCiclo+" NombreCiclo: "+NombreCiclo+" Acronimo: "+Acronimo+" IdCentro: "+IdCentro;
        }
    }
}
