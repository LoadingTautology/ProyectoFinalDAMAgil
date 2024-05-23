using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Diasemanafranjahorarium
{
    public int IdDiaFranja { get; set; }

    public int IdDia { get; set; }

    public int IdFranja { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Diasemana IdDiaNavigation { get; set; } = null!;

    public virtual Franjahorarium IdFranjaNavigation { get; set; } = null!;
}
