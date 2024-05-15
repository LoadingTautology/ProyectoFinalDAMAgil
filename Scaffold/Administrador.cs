using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Administrador
{
    public int IdAdministrador { get; set; }

    public string Dni { get; set; } = null!;

    public virtual ICollection<Centroeducativo> Centroeducativos { get; set; } = new List<Centroeducativo>();

    public virtual Usuario IdAdministradorNavigation { get; set; } = null!;
}
