using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Usuarioscentroeducativo
{
    public int IdUsuariosCentroEducativo { get; set; }

    public int IdCentro { get; set; }

    public int IdUsuario { get; set; }

    public virtual Centroeducativo IdCentroNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
