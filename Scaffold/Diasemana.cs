using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Diasemana
{
    public string Dia { get; set; } = null!;

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}
