using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Aula
{
    public int IdAula { get; set; }

    public int NumeroAula { get; set; }

    public string NombreAula { get; set; } = null!;

    public int AforoMax { get; set; }

    public int IdCentro { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Centroeducativo IdCentroNavigation { get; set; } = null!;
}
