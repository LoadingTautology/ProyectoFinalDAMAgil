using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class CentroEducativoModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string NombreCentro { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string DireccionCentro { get; set; }

    }
}
