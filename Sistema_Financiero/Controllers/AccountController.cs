using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Services;
using System.Data;
using System.Security.Claims;

namespace Sistema_Financiero.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuariosNegocio _usuariosNegocio;

        public AccountController(UsuariosNegocio usuariosNegocio)
        {
            _usuariosNegocio = usuariosNegocio;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Empleados");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contrasena)
        {
            usuario ??= string.Empty;
            contrasena ??= string.Empty;

            try
            {
                // Llamamos a la capa de negocio para validar las credenciales
                DataTable dtUsuario = _usuariosNegocio.MtdValidarUsuario(usuario, contrasena);

                if (dtUsuario.Rows.Count > 0)
                {
                    DataRow fila = dtUsuario.Rows[0];

                    // 1. Extraemos el Nombre del usuario de forma segura tratando de leer "Nombre" o "NombreUsuario"
                    string NombreUsuarioReal = usuario;
                    if (dtUsuario.Columns.Contains("Nombre"))
                        NombreUsuarioReal = fila["Nombre"].ToString() ?? usuario;
                    else if (dtUsuario.Columns.Contains("NombreUsuario"))
                        NombreUsuarioReal = fila["NombreUsuario"].ToString() ?? usuario;

                    // 2. Extraemos el Rol de forma ultra segura para evitar que el sistema tire "ERROR"
                    string NombreRol = "Operador";

                    if (dtUsuario.Columns.Contains("NombreRol"))
                    {
                        NombreRol = fila["NombreRol"].ToString() ?? "Operador";
                    }
                    else if (dtUsuario.Columns.Contains("CodigoRol"))
                    {
                        // Si tu SP de login solo devuelve el ID del rol, lo mapeamos manualmente
                        string codigo = fila["CodigoRol"].ToString() ?? "";
                        if (codigo == "1") NombreRol = "Administrador";
                        else if (codigo == "3") NombreRol = "Supervisor";
                    }

                    // 3. Creamos las credenciales y los permisos para los candados [Authorize]
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, NombreUsuarioReal),
                        new Claim(ClaimTypes.Role, NombreRol)
                    };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    // Iniciamos la sesión en el navegador
                    await HttpContext.SignInAsync("CookieAuth", principal);

                    return RedirectToAction("Index", "Empleados");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña no válidos.";
                }
            }
            catch (Exception ex)
            {
                // Si la base de datos falla, te dirá exactamente qué columna o parámetro causó el problema
                ViewBag.Error = "Error de autenticación: " + ex.Message;
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}