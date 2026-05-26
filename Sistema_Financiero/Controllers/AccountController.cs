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
                    // Extraemos los datos reales de la consulta SQL de forma segura
                    string NombreUsuarioReal = dtUsuario.Rows[0]["Nombre"].ToString() ?? usuario;
                    string NombreRol =   dtUsuario.Rows[0]["NombreRol"].ToString() ?? "Operador";

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, NombreUsuarioReal),
                        new Claim(ClaimTypes.Role, NombreRol)
                    };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("CookieAuth", principal);

                    return RedirectToAction("Index", "Empleados");
                }
                else
                {
                    // DIAGNÓSTICO: Si el SP se ejecuta pero no encuentra coincidencia
                    ViewBag.Error = "Usuario o contraseña no validos";
                }
            }
            catch (Exception)
            {
                // Captura fallas de conexión o errores internos de SQL Server
                ViewBag.Error = "ERROR";
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