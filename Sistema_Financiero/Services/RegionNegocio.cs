using Sistema_Financiero.data;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Logica
{
    public class RegionNegocio
    {
        private readonly RegionDatos _regionDatos;

        public RegionNegocio(RegionDatos regionDatos)
        {
            _regionDatos = regionDatos;
        }
        public bool MtdAgregarRegion(RegionModelo region, out string mensajeSalida)
        {
            return _regionDatos.MtdAgregarRegion(region, out mensajeSalida);
        }

        public List<RegionModelo> MtdConsultarRegion()
        {
            return _regionDatos.MtdConsultarRegion();
        }

        public List<RegionModelo> MtdBuscarRegion(string nombre)
        {
            return _regionDatos.MtdBuscarRegion(nombre);
        }

        public bool MtdEliminarRegion(int Codigo, out string mensajeSalida)
        {
           return _regionDatos.MtdEliminarRegion(Codigo, out mensajeSalida);
        }

        public bool MtdActualizarRegion(RegionModelo region, out string mensajeSalida)
        {
            return _regionDatos.MtdActualizarRegion(region, out mensajeSalida);
        }
    }
}