using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sistema_Financiero.Services;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UsuariosNegocio _usuariosNegocio;
        private readonly EmpleadosNegocio _empleadosNegocio;

        // El constructor ya recibe correctamente las dos capas por Inyección de Dependencias
        public UsuariosController(UsuariosNegocio usuariosNegocio, EmpleadosNegocio empleadosNegocio)
        {
            _usuariosNegocio = usuariosNegocio;
            _empleadosNegocio = empleadosNegocio;
        }

        // ── LISTADO PRINCIPAL ──
        public IActionResult Index(string buscarNombre)
        {
            List<UsuarioModelo> usuario;

            if (!string.IsNullOrEmpty(buscarNombre))
            {
                usuario = _usuariosNegocio.MtdBuscarUsuarios(buscarNombre);
                ViewBag.BusquedaActual = buscarNombre;
            }
            else
            {
                usuario = _usuariosNegocio.MtdConsultarUsuarios();
            }

            return View(usuario);
        }

        // ── CREAR (GET) ──
        public IActionResult Crear()
        {
            try
            {
                // 1. CARGAMOS LOS ROLES (Como ya lo tenías)
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();

                // 2. ¡AQUÍ ESTÁ EL TRUCO! Cargamos los empleados usando la variable privada _empleadosNegocio
                ViewBag.Empleados = _empleadosNegocio.MtdConsultarEmpleados() ?? new List<EmpleadosModelo>();

                return View();
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar el formulario: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── CREAR (POST) ──
        [HttpPost]
        public IActionResult Crear(UsuarioModelo u)
        {
            try
            {
                bool insertado = _usuariosNegocio.MtdInsertarUsuario(u, out string mensajeSalida);

                if (insertado)
                {
                    TempData["MensajeExito"] = mensajeSalida;
                    return RedirectToAction("Index");
                }

                // Si no se pudo insertar, volvemos a llenar AMBOS Comboboxes para no romper la vista
                ViewBag.Error = mensajeSalida;
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                ViewBag.Empleados = _empleadosNegocio.MtdConsultarEmpleados() ?? new List<EmpleadosModelo>(); // <- Agregado aquí

                return View(u);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, también recargamos AMBOS Comboboxes antes de retornar la vista
                ViewBag.Error = "Error inesperado al crear: " + ex.Message;
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                ViewBag.Empleados = _empleadosNegocio.MtdConsultarEmpleados() ?? new List<EmpleadosModelo>(); // <- Agregado aquí

                return View(u);
            }
        }

        // ── EDITAR (GET) ──
        public IActionResult Editar(int id)
        {
            try
            {
                var lista = _usuariosNegocio.MtdConsultarUsuarios();
                var usuario = lista?.FirstOrDefault(u => u.CodigoUsuario == id);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "El usuario seleccionado no existe.";
                    return RedirectToAction("Index");
                }

                usuario.Clave = ""; // no exponemos la clave actual en el HTML

                // 1. Cargamos los Roles
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();

                // 2. ¡OBLIGATORIO PARA EDITAR! Cargamos los empleados para el nuevo Combobox
                ViewBag.Empleados = _empleadosNegocio.MtdConsultarEmpleados() ?? new List<EmpleadosModelo>();

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar los datos del usuario: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── EDITAR (POST) ──
        [HttpPost]
        public IActionResult Editar(UsuarioModelo u)
        {
            try
            {
                bool exito = _usuariosNegocio.MtdEditarUsuario(u, out string mensajeSalida);

                if (exito)
                {
                    TempData["MensajeExito"] = mensajeSalida;
                    return RedirectToAction("Index");
                }

                ViewBag.Error = mensajeSalida;
                // Si hay error de validación en la base de datos, volvemos a cargar ambos combos antes de retornar la vista
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                ViewBag.Empleados = _empleadosNegocio.MtdConsultarEmpleados() ?? new List<EmpleadosModelo>(); // <- Agregado aquí también

                return View(u);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar los cambios: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── ELIMINAR (POST) ──
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                string mensaje = _usuariosNegocio.MtdEliminarUsuario(id);
                TempData["MensajeExito"] = !string.IsNullOrEmpty(mensaje) ? mensaje : "Usuario eliminado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al eliminar el usuario: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}