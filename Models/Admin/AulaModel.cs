﻿namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class AulaModel
    {
        public int IdAula { get; set; }

        public int NumeroAula { get; set; }

        public string NombreAula { get; set; } = null!;

        public int AforoMax { get; set; }

        public int IdCentro { get; set; }


        public override string ToString()
        {
            return "IdAula: "+IdAula+" NumeroAula: "+NumeroAula+" NombreAula: "+NombreAula+" AforoMax: "+AforoMax+" IdCentro: "+IdCentro;
        }

    }
}
