using Sistema_Financiero.data;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.Services
{
    public class EmpleadosNegocio
    {
        private readonly EmpleadosDatos _empleadosDatos;

        public EmpleadosNegocio(EmpleadosDatos empleadosDatos)
        {
            _empleadosDatos = empleadosDatos;
        }

        // 1. PUENTE: AGREGAR
        public bool MtdAgregarEmpleado(EmpleadosModelo empleados, out string mensajeSalida)
        {
            return _empleadosDatos.MtdAgregarEmpleado(empleados, out mensajeSalida);
        }

        // 2. PUENTE: ACTUALIZAR
        public bool MtdActualizarEmpleado(EmpleadosModelo empleados, out string mensajeSalida)
        {
            return _empleadosDatos.MtdActualizarEmpleado(empleados, out mensajeSalida);
        }

        // 3. PUENTE: ELIMINAR
        public bool MtdEliminarEmpleado(int Codigo, out string mensajeSalida)
        {
            return _empleadosDatos.MtdEliminarEmpleado(Codigo, out mensajeSalida);
        }

        // 4. PUENTE: CONSULTAR TODO
        public List<EmpleadosModelo> MtdConsultarEmpleados()
        {
            return _empleadosDatos.MtdConsultarEmpleados();
        }

        // 5. PUENTE: BUSCAR POR NOMBRE
        public List<EmpleadosModelo> MtdBuscarEmpleado(string nombre)
        {
            return _empleadosDatos.MtdBuscarEmpleado(nombre);
        }
    }
}