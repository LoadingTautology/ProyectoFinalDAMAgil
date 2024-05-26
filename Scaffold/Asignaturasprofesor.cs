using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Asignaturasprofesor
{
    public int IdAsignaturasProfesor { get; set; }

    public int IdProfesor { get; set; }

    public int IdAsignaturasCicloFormativo { get; set; }

    public virtual Asignaturascicloformativo IdAsignaturasCicloFormativoNavigation { get; set; } = null!;

    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
