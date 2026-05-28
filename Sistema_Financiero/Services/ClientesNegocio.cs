using Sistema_Financiero.data;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Services
{
    public class ClientesNegocio
    {
        private readonly ClientesDatos _clientes;

        public ClientesNegocio(ClientesDatos clientes)
        {
            _clientes = clientes;
        }
        public bool MtdAgregarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientes.MtdAgregarCliente(clientes, out MensajeSalida);
        }

        public string MtdActualizarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientes.MtdActualizarCliente(clientes, out MensajeSalida);
        }

        public string MtdEliminarCliente(int CodigoCliente)
        {
            return _clientes.MtdEliminarCliente(CodigoCliente);
        }

        public List<ClientesModelo> MtdBuscarCliente( string Nombre)
        {
            string NombreBuscar = Nombre?.Trim() ?? "";
            return _clientes.MtdBuscarCliente(NombreBuscar);
        }

        public List<ClientesModelo> MtdConsultarClientes()
        {
            return _clientes.MtdConsultarCliente();
        }

    }
}
