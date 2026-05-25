using System.ComponentModel.DataAnnotations;
namespace Sistema_Financiero.Models
{
    public class UsuarioModelo
    {
        [Key]
        [Required]
        public int CodigoUsuario { get; set; }
        public int CodigoEmpleado { get; set; }
        public int CodigoRol { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        [Required]
        public string? Clave { get; set; }
        public bool Estado { get; set; }

        // solo para mostrar en la vista, no se guarda en BD
        public string? NombreRol { get; set; }
        public string? NombreEmpleado { get; set; }
    }
}
