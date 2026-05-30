using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;

namespace Sistema_Financiero.Controllers
{
    // Se mantiene el rol original de Clientes para permitir lectura general
    [Authorize(Roles = "Administrador,Supervisor,Operario")]
    public class ClientesController : Controller
    {
        private readonly ClientesNegocio _clientesNegocio;

        // Constructor adaptado a la estructura de Empleados
        public ClientesController(ClientesNegocio clientesNegocio)
        {
            _clientesNegocio = clientesNegocio;
        }

        // =========================================================================
        // LISTADO PRINCIPAL
        // =========================================================================
        public IActionResult Index(string buscarNombre)
        {
            List<ClientesModelo> clientes;

            if (!string.IsNullOrEmpty(buscarNombre))
            {
                clientes = _clientesNegocio.MtdBuscarCliente(buscarNombre);
                ViewBag.BusquedaActual = buscarNombre;
            }
            else
            {
                // Pasar un string vacío al método que requiere el parámetro 'nombre'
                clientes = _clientesNegocio.MtdConsultarClientes(string.Empty);
            }

            return View(clientes);
        }

        // =========================================================================
        // AGREGAR CLIENTE
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            // Cargamos los municipios antes de abrir la vista de Agregar
            ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Agregar(ClientesModelo cliente)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo es inválido, recargamos los municipios antes de retornar la vista
                ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
                return View(cliente);
            }

            bool exito = _clientesNegocio.MtdAgregarCliente(cliente, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            // Si falla la base de datos, también recargamos los municipios
            ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
            ViewBag.Error = mensajeBDD;
            return View(cliente);
        }

        // =========================================================================
        // EDITAR / ACTUALIZAR CLIENTE
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var cliente = _clientesNegocio.MtdConsultarClientes(string.Empty)
                .Find(c => c.CodigoCliente == id);

            if (cliente == null)
            {
                TempData["MensajeError"] = "No se encontró el cliente a editar.";
                return RedirectToAction("Index");
            }

            // Cargamos los municipios también en la vista de Editar por si se requiere cambiar
            ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
            return View(cliente);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Editar(ClientesModelo cliente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
                return View(cliente);
            }

            bool exito = _clientesNegocio.MtdActualizarCliente(cliente, out string mensajeBDD);

            if (exito)
            {
                TempData["MensajeExito"] = mensajeBDD;
                return RedirectToAction("Index");
            }

            ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios();
            ViewBag.Error = mensajeBDD;
            return View(cliente);
        }

        // =========================================================================
        // ELIMINAR CLIENTE
        // =========================================================================
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            bool exito = _clientesNegocio.MtdEliminarCliente(id, out string mensajeBDD);

            if (exito) TempData["MensajeExito"] = mensajeBDD;
            else TempData["MensajeError"] = mensajeBDD;

            return RedirectToAction("Index");
        }
    }
}