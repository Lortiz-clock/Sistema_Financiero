using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Sistema_Financiero.Logica;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Controllers
{
    public class RegionController : Controller
    {
        private readonly RegionNegocio _regionNegocio;

        public RegionController(RegionNegocio regionNegocio)
        {
            _regionNegocio = regionNegocio;
        }

        // ── CONSULTAR (Todos los roles autenticados ven esto) ──
        [Authorize]
        public IActionResult Index(string buscarNombre)
        {
            try
            {
                // Reutilizamos tu lógica de negocio que devuelve List<RegionModelo>
                var lista = _regionNegocio.MtdBuscarRegion(buscarNombre);
                ViewBag.TxtBuscar = buscarNombre;
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<RegionModelo>());
            }
        }

        // ── CREAR (Solo Administrador) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Crear()
        {
            return View(new RegionModelo());
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(RegionModelo region)
        {
            try
            {
                string mensaje;
                bool exito = _regionNegocio.MtdAgregarRegion(region, out mensaje);

                if (exito)
                {
                    return RedirectToAction("Index");
                }

                ViewBag.Error = mensaje;
                return View(region);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(region);
            }
        }

        // ── EDITAR (Solo Administrador) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            try
            {
                // Buscamos la región por su ID filtrando en la lista existente
                var region = _regionNegocio.MtdBuscarRegion("")
                    .FirstOrDefault(r => r.CodigoRegion == id);

                if (region == null) return RedirectToAction("Index");

                return View(region);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(RegionModelo region)
        {
            try
            {
                string mensaje;
                // Reutiliza MtdAgregarRegion para evitar el error de compilación CS1061
                bool exito = _regionNegocio.MtdAgregarRegion(region, out mensaje);

                if (exito)
                {
                    return RedirectToAction("Index");
                }

                ViewBag.Error = mensaje;
                return View(region);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(region);
            }
        }

        // ── ELIMINAR (Solo Administrador) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                string mensaje;
                _regionNegocio.MtdEliminarRegion(id, out mensaje);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}