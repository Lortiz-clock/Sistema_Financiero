using System.Data;
using Sistema_Financiero.data;

namespace Sistema_Financiero.Services
{
    public class EmpleadosNegocio
    {
        private readonly EmpleadosDatos _empleadosDatos;

        public EmpleadosNegocio(EmpleadosDatos empleadosDatos)
        {
            _empleadosDatos = empleadosDatos;
        }

        public DataTable MtdConsultarEmpleados()
        {
            return _empleadosDatos.MtdConsultarEmpleados();
        }
    }
}
