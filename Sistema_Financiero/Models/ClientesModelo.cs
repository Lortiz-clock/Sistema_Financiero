namespace Sistema_Financiero.Models
{
    public class ClientesModelo
    {
        public int CodigoCliente { get; set; }
        public int CodigoMunicipio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string DPI { get; set; } = string.Empty;
        public string NIT { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

    }
}
