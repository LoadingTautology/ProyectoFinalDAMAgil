using System;
using System.Collections.Generic;

namespace ProyectoFinalDAMAgil.Scaffold;

public partial class Centroeducativo
{
    public int IdCentro { get; set; }

    public string NombreCentro { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int IdAdministrador { get; set; }

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    public virtual ICollection<Cicloformativo> Cicloformativos { get; set; } = new List<Cicloformativo>();

    public virtual Administrador IdAdministradorNavigation { get; set; } = null!;

    public virtual ICollection<Usuarioscentroeducativo> Usuarioscentroeducativos { get; set; } = new List<Usuarioscentroeducativo>();
}
