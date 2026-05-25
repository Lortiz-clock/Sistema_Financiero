    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Data;
    using Sistema_Financiero.Services;
    using Sistema_Financiero.Models;

    namespace Sistema_Financiero.Controllers
    {
        public class UsuariosController : Controller
        {
            private readonly UsuariosNegocio _usuariosNegocio;

            public UsuariosController(UsuariosNegocio usuariosNegocio)
            {
                _usuariosNegocio = usuariosNegocio;
            }

            // ── CONSULTAR (todos ven esto) ──
            public IActionResult Index()
            {
                try
                {
                    DataTable dt = _usuariosNegocio.MtdConsultarUsuarios();
                    return View(dt);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(new DataTable());
                }
            }

            // ── CREAR ──
            [Authorize(Roles = "Administrador")] // solo Admin (CodigoRol = 1)
            public IActionResult Crear()
            {
                try
                {
                    ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles();
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }

            [HttpPost]
            [Authorize(Roles = "Administrador")]
            public IActionResult Crear(UsuarioModelo u)
            {
                try
                {
                    _usuariosNegocio.MtdInsertarUsuario(u);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles();
                    return View(u);
                }
            }

            // ── EDITAR ──
            [Authorize(Roles = "Administrador")]
            public IActionResult Editar(int id)
            {
                try
                {
                    DataTable dt = _usuariosNegocio.MtdConsultarUsuarios();
                    DataRow? fila = dt.AsEnumerable()
                        .FirstOrDefault(r => Convert.ToInt32(r["CodigoUsuario"]) == id);

                    if (fila == null) return RedirectToAction("Index");

                    UsuarioModelo u = new UsuarioModelo
                    {
                        CodigoUsuario = Convert.ToInt32(fila["CodigoUsuario"]),
                        CodigoEmpleado = Convert.ToInt32(fila["CodigoEmpleado"]),
                        CodigoRol = Convert.ToInt32(fila["CodigoRol"]),
                        Nombre = fila["NombreUsuario"].ToString() ?? "",
                        Estado = Convert.ToBoolean(fila["Estado"])
                    };

                    ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles();
                    return View(u);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return RedirectToAction("Index");
                }
            }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(UsuarioModelo u)
        {
            try
            {
                _usuariosNegocio.MtdActualizarUsuario(u);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = _usuariosNegocio.MtdConsultarRoles();
                return View(u);
            }
        }

        // ── ELIMINAR ──
        [Authorize(Roles = "Administrador")]
            public IActionResult Eliminar(int id)
            {
                try
                {
                    _usuariosNegocio.MtdEliminarUsuario(id);
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
