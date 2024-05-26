using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Matriculasalumno
{
    public int IdMatriculasAlumnos { get; set; }

    public int IdAlumno { get; set; }

    public int IdAsignaturasCicloFormativo { get; set; }

    public float Eva1 { get; set; }

    public float Eva2 { get; set; }

    public float Eva3 { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Asignaturascicloformativo IdAsignaturasCicloFormativoNavigation { get; set; } = null!;
}
