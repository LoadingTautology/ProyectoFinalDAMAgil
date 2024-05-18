using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Asignaturascicloformativo
{
    public int IdAsignaturasCicloFormativo { get; set; }

    public int IdAsignatura { get; set; }

    public int IdCiclo { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Asignatura IdAsignaturaNavigation { get; set; } = null!;

    public virtual Cicloformativo IdCicloNavigation { get; set; } = null!;
}
