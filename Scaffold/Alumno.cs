using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    public DateOnly FechaDeNacimiento { get; set; }

    public int IdCentro { get; set; }

    public virtual Usuario IdAlumnoNavigation { get; set; } = null!;

    public virtual Centroeducativo IdCentroNavigation { get; set; } = null!;

    public virtual ICollection<Matriculasalumno> Matriculasalumnos { get; set; } = new List<Matriculasalumno>();
}
