using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;

namespace Sistema_Financiero.Controllers
{
    [Authorize(Roles = "Administrador,Supervisor")]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadosNegocio _empleadosNegocio;
        private readonly SucursalesNegocio _sucursalesNegocio;
        public EmpleadosController(EmpleadosNegocio empleadosNegocio, SucursalesNegocio sucursalesNegocio)
        {
            _empleadosNegocio = empleadosNegocio;
            _sucursalesNegocio = sucursalesNegocio;
        }

        // =========================================================================
        // LISTADO PRINCIPAL
        // =========================================================================
        public IActionResult Index(string buscarNombre)
        {
            List<EmpleadosModelo> empleados;

            if (!string.IsNullOrEmpty(buscarNombre))
            {
                empleados = _empleadosNegocio.MtdBuscarEmpleado(buscarNombre);
                ViewBag.BusquedaActual = buscarNombre;
            }
            else
            {
                empleados = _empleadosNegocio.MtdConsultarEmpleados();
            }

            return View(empleados);
        }

        // =========================================================================
        // AGREGAR EMPLEADO
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            // 3. Cargamos las sucursales antes de abrir la vista de Agregar
            ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Agregar(EmpleadosModelo empleado)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo es inválido, debemos recargar las sucursales antes de retornar la vista
                ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
                return View(empleado);
            }

            bool exito = _empleadosNegocio.MtdAgregarEmpleado(empleado, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            // Si falla la base de datos, también recargamos las sucursales
            ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
            ViewBag.Error = mensajeBDD;
            return View(empleado);
        }

        // =========================================================================
        // EDITAR / ACTUALIZAR EMPLEADO
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var empleado = _empleadosNegocio.MtdConsultarEmpleados()
                .Find(e => e.CodigoEmpleado == id);

            if (empleado == null)
            {
                TempData["MensajeError"] = "No se encontró el empleado a editar.";
                return RedirectToAction("Index");
            }

            // 4. Cargamos las sucursales también en la vista de Editar para poder reasignar sucursal si se requiere
            ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
            return View(empleado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Editar(EmpleadosModelo empleado)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
                return View(empleado);
            }

            bool exito = _empleadosNegocio.MtdActualizarEmpleado(empleado, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            ViewBag.Sucursales = _sucursalesNegocio.MtdConsultarSucursal();
            ViewBag.Error = mensajeBDD;
            return View(empleado);
        }

        // =========================================================================
        // ELIMINAR EMPLEADO
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            bool exito = _empleadosNegocio.MtdEliminarEmpleado(id, out string mensajeBDD);

            if (exito) TempData["MensajeExito"] = mensajeBDD;
            else TempData["MensajeError"] = mensajeBDD;

            return RedirectToAction("Index");
        }
    }
}