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

        public EmpleadosController(EmpleadosNegocio empleadosNegocio)
        {
            _empleadosNegocio = empleadosNegocio;
        }

        // =========================================================================
        // LISTADO PRINCIPAL
        // =========================================================================
        public IActionResult Index(string buscarNombre)
        {
            List<EmpleadosModelo> empleados;

            // Si el cuadro de texto no está vacío, usamos tu SP de Buscar
            if (!string.IsNullOrEmpty(buscarNombre))
            {
                empleados = _empleadosNegocio.MtdBuscarEmpleado(buscarNombre);
                // Guardamos lo que buscó el usuario para volverlo a pintar en la barra de texto
                ViewBag.BusquedaActual = buscarNombre;
            }
            else
            {
                // Si está vacío, cargamos la lista completa con tu SP de Consultar
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
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Agregar(EmpleadosModelo empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            bool exito = _empleadosNegocio.MtdAgregarEmpleado(empleado, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            ViewBag.Error = mensajeBDD;
            return View(empleado);
        }

        // =========================================================================
        // EDITAR / ACTUALIZAR EMPLEADO
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            // Buscamos al empleado para cargar sus datos en el formulario
            var empleado = _empleadosNegocio.MtdConsultarEmpleados()
                .Find(e => e.CodigoEmpleado == id);

            if (empleado == null)
            {
                TempData["MensajeError"] = "No se encontró el empleado a editar.";
                return RedirectToAction("Index");
            }

            return View(empleado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Editar(EmpleadosModelo empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            bool exito = _empleadosNegocio.MtdActualizarEmpleado(empleado, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

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