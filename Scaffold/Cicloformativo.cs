using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Cicloformativo
{
    public int IdCiclo { get; set; }

    public string NombreCiclo { get; set; } = null!;

    public string Acronimo { get; set; } = null!;

    public int IdCentro { get; set; }

    public virtual ICollection<Asignaturascicloformativo> Asignaturascicloformativos { get; set; } = new List<Asignaturascicloformativo>();

    public virtual Centroeducativo IdCentroNavigation { get; set; } = null!;
}
