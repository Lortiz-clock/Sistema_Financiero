using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;

namespace Sistema_Financiero.Controllers
{
    // Permitimos que entren Administradores, Supervisores y Operarios a nivel general
    [Authorize(Roles = "Administrador,Supervisor,Operario")]
    public class ClientesController : Controller
    {
        private readonly ClientesNegocio _clientesNegocio;

        public ClientesController(ClientesNegocio clientesNegocio)
        {
            _clientesNegocio = clientesNegocio;
        }

        // Acceso para todos los roles autorizados
        public IActionResult Index(string buscarNombre)
        {
            List<ClientesModelo> lista;
            ViewBag.BusquedaActual = buscarNombre;

            if (!string.IsNullOrEmpty(buscarNombre))
            {
                lista = _clientesNegocio.MtdBuscarCliente(buscarNombre);
            }
            else
            {
                lista = _clientesNegocio.MtdConsultarClientes();
            }

            return View(lista);
        }

        // =========================================================================
        // ACCIONES RESTRINGIDAS: SOLO PARA EL ROL "Administrador"
        // =========================================================================

        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar(ClientesModelo cliente)
        {
            if (_clientesNegocio.MtdAgregarCliente(cliente, out string mensajeSalida))
            {
                TempData["MensajeExito"] = mensajeSalida;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensajeSalida;
                return View(cliente);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var cliente = _clientesNegocio.MtdConsultarClientes()
                            .FirstOrDefault(c => c.CodigoCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(ClientesModelo cliente)
        {
            string resultado = _clientesNegocio.MtdActualizarCliente(cliente, out string mensajeSalida);

            if (!string.IsNullOrEmpty(mensajeSalida) && !mensajeSalida.Contains("error", StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeExito"] = mensajeSalida;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensajeSalida;
                return View(cliente);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                string mensaje = _clientesNegocio.MtdEliminarCliente(id);
                TempData["MensajeExito"] = mensaje;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}