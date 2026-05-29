using Sistema_Financiero.data;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Services
{
    public class ClientesNegocio
    {
        private readonly ClientesDatos _clientesDatos;
        private readonly MunicipioDatos _municipioDatos; // ◄ Agregamos la conexión a los datos de municipio

        // El constructor ahora recibe ambas clases de datos de forma limpia
        public ClientesNegocio(ClientesDatos clientesDatos, MunicipioDatos municipioDatos)
        {
            _clientesDatos = clientesDatos;
            _municipioDatos = municipioDatos;
        }

        public bool MtdAgregarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientesDatos.MtdAgregarCliente(clientes, out MensajeSalida);
        }

        public string MtdActualizarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientesDatos.MtdActualizarCliente(clientes, out MensajeSalida);
        }

        public string MtdEliminarCliente(int CodigoCliente)
        {
            return _clientesDatos.MtdEliminarCliente(CodigoCliente);
        }

        public List<ClientesModelo> MtdBuscarCliente(string Nombre)
        {
            string NombreBuscar = Nombre?.Trim() ?? "";
            return _clientesDatos.MtdBuscarCliente(NombreBuscar);
        }

        public List<ClientesModelo> MtdConsultarClientes()
        {
            return _clientesDatos.MtdConsultarCliente();
        }

        // CORREGIDO: Ahora llama directamente a la clase correcta 'MunicipioDatos'
        // y al método exacto 'MtdConsultarMunicipio()' que nos compartiste
        public List<MunicipioModelo> MtdConsultarMunicipios()
        {
            return _municipioDatos.MtdConsultarMunicipio();
        }
    }
}