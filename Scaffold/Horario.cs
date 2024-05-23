using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Horario
{
    public int IdHorario { get; set; }

    public int IdAula { get; set; }

    public int IdDiaFranja { get; set; }

    public int IdAsignatura { get; set; }

    public int IdEstudio { get; set; }

    public string ColorAsignatura { get; set; } = null!;

    public virtual Asignaturascicloformativo Asignaturascicloformativo { get; set; } = null!;

    public virtual Aula IdAulaNavigation { get; set; } = null!;

    public virtual Diasemanafranjahorarium IdDiaFranjaNavigation { get; set; } = null!;
}
