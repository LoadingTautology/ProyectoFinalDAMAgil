﻿using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdAula { get; set; }

    public TimeOnly HoraMinInicio { get; set; }

    public TimeOnly HoraMinFinal { get; set; }

    public string Dia { get; set; } = null!;

    public int IdAsignatura { get; set; }

    public int IdEstudio { get; set; }

    public virtual Asignaturascicloformativo Asignaturascicloformativo { get; set; } = null!;

    public virtual Diasemana DiaNavigation { get; set; } = null!;

    public virtual Franjahorarium Franjahorarium { get; set; } = null!;

    public virtual Aula IdAulaNavigation { get; set; } = null!;
}
