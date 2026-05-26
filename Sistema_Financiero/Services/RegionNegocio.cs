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
        public string MtdAgregarRegion(RegionModelo region)
        {
            if (region == null)
                throw new Exception("No se resibieron datos");
            if (string.IsNullOrWhiteSpace(region.Nombre))
                throw new Exception("El nombre de la región no puede estar vacío.");
            try
            {
                return _regionDatos.MtdAgregarRegion(region);
            }
            catch 
            {
                throw;
            }
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

        public string MtdEditarRegion(RegionModelo region)
        {
            if (region == null)
                throw new Exception("No se resibieron datos");
            if (region.CodigoRegion <= 0)
                throw new Exception("Debe enviar el codigo de la region");

            try
            {
                return _regionDatos.MtdEditarRegion(region);
            }
            catch 
            {

                throw;
            }
        }
    }
}