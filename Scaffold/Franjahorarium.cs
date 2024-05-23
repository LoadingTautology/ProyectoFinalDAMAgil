using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Franjahorarium
{
    public int IdFranja { get; set; }

    public TimeOnly HoraMinInicio { get; set; }

    public TimeOnly HoraMinFinal { get; set; }

    public virtual ICollection<Diasemanafranjahorarium> Diasemanafranjahoraria { get; set; } = new List<Diasemanafranjahorarium>();
}
