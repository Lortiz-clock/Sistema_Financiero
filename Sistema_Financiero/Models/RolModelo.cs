using System.ComponentModel.DataAnnotations;

namespace Sistema_Financiero.Models
{
    public class RolModelo
    {
        [Key]
        public int CodigoRol { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}