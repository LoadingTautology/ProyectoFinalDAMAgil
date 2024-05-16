using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDAMAgil.Models
{
    public class UsuarioCorreo
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string NombreUsuario { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string ApellidosUsuario { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string DNI { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        //[DataType(DataType.Password)]
        public string Clave { get; set; } = null!;

    }
}
