using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Diasemana
{
    public int IdDia { get; set; }

    public string DiaSemana1 { get; set; } = null!;

    public virtual ICollection<Diasemanafranjahorarium> Diasemanafranjahoraria { get; set; } = new List<Diasemanafranjahorarium>();
}
