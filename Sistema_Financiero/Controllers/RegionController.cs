using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Logica;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;
using System.Data;

namespace Sistema_Financiero.Controllers
{
    public class RegionController : Controller
    {
        private readonly RegionNegocio _regionNegocio;

        public RegionController(RegionNegocio regionNegocio)
        {
            _regionNegocio = regionNegocio;
        }

        // ── CONSULTAR ──
        [Authorize]
        public IActionResult Index(string buscarNombre)
        {
            buscarNombre ??= string.Empty;

            try
            {
                var lista = _regionNegocio.MtdBuscarRegion(buscarNombre);
                ViewBag.BusquedaActual = buscarNombre; // Usando tu estándar de Empleados
                return View(lista);
            }
            catch (Exception)
            {
                ViewBag.Error = "ERROR al cargar las regiones";
                return View(new List<RegionModelo>());
            }
        }

        // ── CREAR (GET) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View(new RegionModelo());
        }

        // ── CREAR (POST) ──
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(RegionModelo region)
        {
            try
            {
                // INTEGRACIÓN PERFECTA CON TU LÓGICA:
                // Llama a tu método real que recibe 1 solo argumento y devuelve un string.
                string mensajeExito = _regionNegocio.MtdAgregarRegion(region);

                TempData["MensajeExito"] = "Región creada con éxito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Si salta un throw ("No se recibieron datos" o "El nombre no puede estar vacío")
                // lo atrapamos limpiamente aquí para mostrarlo en la vista.
                ViewBag.Error = ex.Message;
                return View(region);
            }
        }
        // ── EDITAR (GET) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            try
            {
                var listaRegiones = _regionNegocio.MtdBuscarRegion("");
                RegionModelo? regionSeleccionada = null;

                foreach (var r in listaRegiones)
                {
                    if (r.CodigoRegion == id)
                    {
                        regionSeleccionada = r;
                        break;
                    }
                }

                if (regionSeleccionada == null)
                {
                    return RedirectToAction("Index");
                }

                return View(regionSeleccionada);
            }
            catch (Exception)
            {
                ViewBag.Error = "ERROR al cargar la región seleccionada";
                return RedirectToAction("Index");
            }
        }

        // ── EDITAR (POST) ──
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(RegionModelo region)
        {
            try
            {
                string mensajeExito = _regionNegocio.MtdEditarRegion(region);
                TempData["MensajeExito"] = "Región actualizada con éxito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(region);
            }
        }

        // ── ELIMINAR ──
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                string mensaje;
                _regionNegocio.MtdEliminarRegion(id, out mensaje);
                TempData["MensajeExito"] = "Región eliminada con éxito";
            }
            catch (Exception)
            {
                ViewBag.Error = "ERROR al eliminar la región";
            }

            return RedirectToAction("Index");
        }
    }
}