using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Profesor
{
    public int IdProfesor { get; set; }

    public string Especialidad { get; set; } = null!;

    public int IdCentro { get; set; }

    public virtual Centroeducativo IdCentroNavigation { get; set; } = null!;

    public virtual Usuario IdProfesorNavigation { get; set; } = null!;
}
