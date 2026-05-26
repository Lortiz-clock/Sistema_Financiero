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

        public bool MtdAgregarRegion(RegionModelo region, out string MensajeSalida)
        {
            if (region == null)
            {
                MensajeSalida = "Los datos de la región no pueden ser nulos.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(region.Nombre))
            {
                MensajeSalida = "El nombre de la región es obligatorio.";
                return false;
            }

            return _regionDatos.MtdAgregarRegion(region, out MensajeSalida);
        }

        public List<RegionModelo> MtdConsultarRegion()
        {
            return _regionDatos.MtdConsultarRegion();
        }

        public List<RegionModelo> MtdBuscarRegion(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return _regionDatos.MtdConsultarRegion();
            }

            return _regionDatos.MtdBuscarRegion(nombre.Trim());
        }

        public bool MtdEliminarRegion(int Codigo, out string mensajeSalida)
        {
            if (Codigo <= 0)
            {
                mensajeSalida = "El código de la región no es válido.";
                return false;
            }

            return _regionDatos.MtdEliminarRegion(Codigo, out mensajeSalida);
        }
    }
}