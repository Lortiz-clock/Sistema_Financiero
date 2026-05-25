using System.ComponentModel.DataAnnotations;

namespace Sistema_Financiero.Models
{
    public class EmpleadosModelo
    {
        [Key]
        [Required]
        public int CodigoEmpleado { get; set; }
        public int CodigoSucursal { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Cargo { get; set; }
        public bool Estado { get; set; }
    }
}
