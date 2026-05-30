using Sistema_Financiero.data;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.Services
{
    public class SucursalesNegocio
    {
        private readonly SucursalesDatos _sucursalDatos;
        public SucursalesNegocio(SucursalesDatos sucursalDatos)
        {
            _sucursalDatos = sucursalDatos;
        }

        public bool MtdAgregarSucursal(SucursalesModelo sucursalDatos, out string MensajeSalida)
        {
            return _sucursalDatos.MtdAgregarSucursal(sucursalDatos, out MensajeSalida);
        }


        public bool MtdEditarSucursal(SucursalesModelo sucursalDatos, out string MensajeSalida)
        {
            return _sucursalDatos.MtdActualizarSucursal(sucursalDatos, out MensajeSalida);
        }


        public bool MtdEliminarSucursal(int CodigoSucursal)
        {

            return _sucursalDatos.MtdEliminarSucursal(CodigoSucursal, out string mensajeSalida);
        }

        public List<SucursalesModelo> MtdBuscarSucursal(string Nombre)
        {
            return _sucursalDatos.MtdBuscarSucursal(Nombre);
        }

        public List<SucursalesModelo> MtdConsultarSucursal()
        {
            return _sucursalDatos.MtdConsultarSucursal();
        }
    }
}