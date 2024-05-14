using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Correoelectronico
{
    public string Email { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
