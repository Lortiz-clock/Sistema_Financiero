using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;

namespace Sistema_Financiero.Controllers
{
    // Permitimos que tanto Administradores como Supervisores puedan ver el listado
    [Authorize(Roles = "Administrador,Supervisor")]
    public class SucursalesController : Controller
    {
        private readonly SucursalesNegocio _sucursalNegocio;

        public SucursalesController(SucursalesNegocio sucursalNegocio)
        {
            _sucursalNegocio = sucursalNegocio;
        }

        // =========================================================================
        // LISTADO PRINCIPAL
        // =========================================================================
        public IActionResult Index(string buscarNombre)
        {
            List<SucursalesModelo> sucursales;

            // Si el cuadro de texto no está vacío, usamos tu SP de Buscar
            if (!string.IsNullOrEmpty(buscarNombre))
            {
                sucursales = _sucursalNegocio.MtdBuscarSucursal(buscarNombre);
                ViewBag.BusquedaActual = buscarNombre;
            }
            else
            {
                // Si está vacío, cargamos la lista completa con tu SP de Consultar
                sucursales = _sucursalNegocio.MtdConsultarSucursal();
            }

            return View(sucursales);
        }

        // =========================================================================
        // AGREGAR SUCURSAL
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Agregar(SucursalesModelo sucursal)
        {
            if (!ModelState.IsValid) return View(sucursal);

            string mensajeBDD;
            bool exito = _sucursalNegocio.MtdAgregarSucursal(sucursal, out mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            ViewBag.Error = mensajeBDD;
            return View(sucursal);
        }

        // =========================================================================
        // EDITAR SUCURSAL
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            // Buscamos la sucursal para cargar sus datos en el formulario
            var sucursal = _sucursalNegocio.MtdConsultarSucursal()
                .Find(s => s.CodigoSucursal == id);

            if (sucursal == null)
            {
                TempData["MensajeError"] = "No se encontró la sucursal a editar.";
                return RedirectToAction("Index");
            }

            return View(sucursal);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Editar(SucursalesModelo sucursal)
        {
            if (!ModelState.IsValid) return View(sucursal);

            string mensajeBDD;
            // Tu método de negocio de Editar devuelve un string
            _sucursalNegocio.MtdEditarSucursal(sucursal, out mensajeBDD);

            // Verificamos si el procedimiento fue exitoso basándonos en tu lógica
            if (!mensajeBDD.ToLower().Contains("error"))
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            ViewBag.Error = mensajeBDD;
            return View(sucursal);
        }

        // =========================================================================
        // ELIMINAR SUCURSAL
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            // Tu método de negocio elimina y devuelve el string con el mensaje del SP
            string mensajeBDD = _sucursalNegocio.MtdEliminarSucursal(id);

            if (!mensajeBDD.ToLower().Contains("error"))
                TempData["MensajeExito"] = mensajeBDD;
            else
                TempData["MensajeError"] = mensajeBDD;

            return RedirectToAction("Index");
        }
    }
}