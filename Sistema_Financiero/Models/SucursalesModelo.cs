using System.ComponentModel.DataAnnotations;

namespace Sistema_Financiero.Models
{
    public class SucursalesModelo
    {
        [Key]
        [Required]
        public int CodigoSucursal { get; set; }
        public int CodigoMunicipio { get; set; }
        public string ?Nombre { get; set; }
        public string ?Direccion { get; set; }
        public string ?Telefono { get; set; }
        public bool ?Estado { get; set; }
    }
}
