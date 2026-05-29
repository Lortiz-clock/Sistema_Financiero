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

        // 1. Esta propiedad es vital para la INSERCIÓN y EDICIÓN (Mapea con name="Nombre" en el formulario)
        public string? Nombre { get; set; } = string.Empty;

        // 2. Esta propiedad es vital para la CONSULTA (Mapea con u.Nombre AS NombreUsuario de tu SP)
        public string? NombreUsuario { get; set; }

        [Required]
        public string? Clave { get; set; }

        public bool Estado { get; set; }

        // Columnas extras provenientes de los INNER JOINs de tu SP
        public string? NombreRol { get; set; }
        public string? NombreEmpleado { get; set; }
    }
}