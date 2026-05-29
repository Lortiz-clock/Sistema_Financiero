using Sistema_Financiero.data;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Services
{
    public class ClientesNegocio
    {
        private readonly ClientesDatos _clientesDatos;
        private readonly MunicipioDatos _municipioDatos;
        
        public ClientesNegocio(ClientesDatos clientesDatos, MunicipioDatos municipioDatos)
        {
            _clientesDatos = clientesDatos;
            _municipioDatos = municipioDatos;
        }

        public bool MtdAgregarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientesDatos.MtdAgregarCliente(clientes, out MensajeSalida);
        }

        public bool MtdActualizarCliente(ClientesModelo clientes, out string MensajeSalida)
        {
            return _clientesDatos.MtdActualizarCliente(clientes, out MensajeSalida);
        }

        public bool MtdEliminarCliente(int CodigoCliente, out string MensajesSalida)
        {
            return _clientesDatos.MtdEliminarCliente(CodigoCliente, out MensajesSalida);
        }

        public List<ClientesModelo> MtdBuscarCliente(string Nombre)
        {
            return _clientesDatos.MtdBuscarCliente(Nombre);
        }

        public List<ClientesModelo> MtdConsultarClientes()
        {
            return _clientesDatos.MtdBuscarCliente();
        }

        // CORREGIDO: Ahora llama directamente a la clase correcta 'MunicipioDatos'
        // y al método exacto 'MtdConsultarMunicipio()' que nos compartiste
        public List<MunicipioModelo> MtdConsultarMunicipios()
        {
            return _municipioDatos.MtdConsultarMunicipio();
        }
    }
}