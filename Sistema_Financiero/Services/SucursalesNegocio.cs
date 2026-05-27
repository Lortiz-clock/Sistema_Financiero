using Sistema_Financiero.data;
using Sistema_Financiero.Models;

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

   
    public string MtdEditarSucursal(SucursalesModelo sucursalDatos, out string MensajeSalida)
    {
        
        return _sucursalDatos.MtdEditarSucursal(sucursalDatos, out MensajeSalida);
    }

   
    public string MtdEliminarSucursal(int CodigoSucursal)
    {
        
        return _sucursalDatos.MtdEliminarSucursal(CodigoSucursal);
    }

    public List<SucursalesModelo> MtdBuscarSucursal(string Nombre)
    {
        string NombreBuscar = Nombre?.Trim() ?? "";
        return _sucursalDatos.MtdBuscarSucursal(NombreBuscar);
    }

    public List<SucursalesModelo> MtdConsultarSucursal()
    {
        return _sucursalDatos.MtdConsultarSucursal();
    }
}