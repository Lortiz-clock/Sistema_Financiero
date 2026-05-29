using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sistema_Financiero.Services;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Controllers
{
    // Restringido completamente al Administrador a nivel de clase
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UsuariosNegocio _usuariosNegocio;

        public UsuariosController(UsuariosNegocio usuariosNegocio)
        {
            _usuariosNegocio = usuariosNegocio;
        }

        // ── LISTADO PRINCIPAL (INDEX) ──
        public IActionResult Index()
        {
            try
            {
                // Manejo seguro de nulabilidad usando el operador de coalescencia
                List<UsuarioModelo> lista = _usuariosNegocio.MtdConsultarUsuarios() ?? new List<UsuarioModelo>();
                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al consultar la lista: " + ex.Message;
                return View(new List<UsuarioModelo>());
            }
        }

        // ── CREAR USUARIO (GET) ──
        public IActionResult Crear()
        {
            try
            {
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                return View();
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar el formulario de creación: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── CREAR USUARIO (POST) ──
        [HttpPost]
        public IActionResult Crear(UsuarioModelo u)
        {
            try
            {
                string mensajeSalida = "";
                // Acoplado al método bool con parámetro OUT de tu nueva capa de negocio
                bool insertado = _usuariosNegocio.MtdInsertarUsuario(u, out mensajeSalida);

                if (insertado)
                {
                    TempData["MensajeExito"] = mensajeSalida;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = mensajeSalida;
                    ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                    return View(u);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error inesperado al crear: " + ex.Message;
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                return View(u);
            }
        }

        // ── EDITAR USUARIO (GET) ──
        public IActionResult Editar(int id)
        {
            try
            {
                var listaUsuarios = _usuariosNegocio.MtdConsultarUsuarios();
                if (listaUsuarios == null)
                {
                    TempData["MensajeError"] = "No se pudo recuperar la lista de usuarios desde el servidor.";
                    return RedirectToAction("Index");
                }

                // Buscamos con LINQ sobre la lista genérica tipada
                var usuario = listaUsuarios.FirstOrDefault(u => u.CodigoUsuario == id);

                if (usuario == null)
                {
                    TempData["MensajeError"] = "El usuario seleccionado no existe.";
                    return RedirectToAction("Index");
                }

                // Evitamos mandar la contraseña actual en texto plano al HTML por seguridad
                usuario.Clave = "";

                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles() ?? new List<RolModelo>();
                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al cargar los datos del usuario: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── EDITAR USUARIO (POST) ──
        [HttpPost]
        public IActionResult Editar(UsuarioModelo u)
        {
            try
            {
                string mensajeSalida = "";
                // Sincronizado con la firma exacta string MtdEditarUsuario(..., out string) de Negocio
                _usuariosNegocio.MtdEditarUsuario(u, out mensajeSalida);

                // Si tu SP gestiona la salida por medio de @Mensaje, lo enviamos directo al TempData
                TempData["MensajeExito"] = !string.IsNullOrEmpty(mensajeSalida) ? mensajeSalida : "Usuario actualizado con éxito.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar los cambios: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ── ELIMINAR USUARIO ──
        public IActionResult Eliminar(int id)
        {
            try
            {
                // El método retorna el mensaje devuelto por el parámetro OUTPUT de la base de datos
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