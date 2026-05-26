using System.ComponentModel.DataAnnotations;

namespace Sistema_Financiero.Models
{
    public class RegionModelo //modelo es entidad
    {
        [Key]
        public int CodigoRegion { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
