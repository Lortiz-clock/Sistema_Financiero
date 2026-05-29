using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Models;
using Sistema_Financiero.Services;

namespace Sistema_Financiero.Controllers
{
    // Permitimos la lectura general para Administradores, Supervisores y Operarios
    [Authorize(Roles = "Administrador,Supervisor,Operario")]
    public class ClientesController : Controller
    {
        private readonly ClientesNegocio _clientesNegocio;

        public ClientesController(ClientesNegocio clientesNegocio)
        {
            _clientesNegocio = clientesNegocio;
        }

        // ── LISTADO PRINCIPAL (INDEX) ──
        public IActionResult Index(string buscarNombre)
        {
            try
            {
                List<ClientesModelo> lista;
                ViewBag.BusquedaActual = buscarNombre;

                if (!string.IsNullOrEmpty(buscarNombre))
                {
                    lista = _clientesNegocio.MtdBuscarCliente(buscarNombre) ?? new List<ClientesModelo>();
                }
                else
                {
                    lista = _clientesNegocio.MtdConsultarClientes() ?? new List<ClientesModelo>();
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar la lista de clientes: " + ex.Message;
                return View(new List<ClientesModelo>());
            }
        }

        // =========================================================================
        // ACCIONES RESTRINGIDAS: SOLO PARA EL ROL "Administrador"
        // =========================================================================

        // ── CREAR CLIENTE (GET) ──
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar()
        {
            try
            {
                // Cargamos los municipios para el ComboBox en la vista de creación
                var listaMunicipios = _clientesNegocio.MtdConsultarMunicipios();
                ViewBag.Municipios = listaMunicipios ?? new List<MunicipioModelo>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al preparar el formulario de creación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── CREAR CLIENTE (POST) ──
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Agregar(ClientesModelo cliente)
        {
            try
            {
                if (_clientesNegocio.MtdAgregarCliente(cliente, out string mensajeSalida))
                {
                    TempData["MensajeExito"] = mensajeSalida;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensajeSalida;
                    // Recargamos municipios si hay un error de validación para no romper el ComboBox
                    ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios() ?? new List<MunicipioModelo>();
                    return View(cliente);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error inesperado al registrar el cliente: " + ex.Message;
                ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios() ?? new List<MunicipioModelo>();
                return View(cliente);
            }
        }

        // ── EDITAR CLIENTE (GET) ──
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            try
            {
                var listaClientes = _clientesNegocio.MtdConsultarClientes();
                if (listaClientes == null)
                {
                    TempData["MensajeError"] = "No se pudo recuperar la lista de clientes.";
                    return RedirectToAction("Index");
                }

                // Buscamos el cliente por su ID en la lista genérica
                var cliente = listaClientes.FirstOrDefault(c => c.CodigoCliente == id);

                if (cliente == null)
                {
                    TempData["MensajeError"] = "El cliente seleccionado no existe.";
                    return RedirectToAction("Index");
                }

                // Cargamos los municipios mapeados desde tu clase MunicipioDatos
                var listaMunicipios = _clientesNegocio.MtdConsultarMunicipios();
                ViewBag.Municipios = listaMunicipios ?? new List<MunicipioModelo>();

                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar los datos del cliente: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── EDITAR CLIENTE (POST) ──
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(ClientesModelo cliente)
        {
            try
            {
                string mensajeSalida = "";
                _clientesNegocio.MtdActualizarCliente(cliente, out mensajeSalida);

                // Evaluación limpia del mensaje devuelto por la base de datos
                if (!string.IsNullOrEmpty(mensajeSalida) && !mensajeSalida.Contains("error", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["MensajeExito"] = mensajeSalida;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = !string.IsNullOrEmpty(mensajeSalida) ? mensajeSalida : "Error al actualizar el cliente.";
                    ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios() ?? new List<MunicipioModelo>();
                    return View(cliente);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado al guardar los cambios: " + ex.Message;
                ViewBag.Municipios = _clientesNegocio.MtdConsultarMunicipios() ?? new List<MunicipioModelo>();
                return View(cliente);
            }
        }

        // ── ELIMINAR CLIENTE (POST) ──
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
                TempData["MensajeError"] = "Error al eliminar el cliente: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}