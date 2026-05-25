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
            [Authorize(Roles = "1")] // solo Admin (CodigoRol = 1)
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
            [Authorize(Roles = "1")]
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
            [Authorize(Roles = "1")]
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
            [Authorize(Roles = "1")]
            public IActionResult Editar(UsuarioModelo u)
            {
            try
            {
                // Si el Administrador escribió algo en la caja de contraseña, se la asignamos al modelo
                // Si la dejó vacía, la propiedad quedará nula o vacía y la base de datos sabrá que no debe cambiarla
                if (!string.IsNullOrEmpty(NuevaClave))
                {
                    // Nota: Si tu modelo no tiene la propiedad 'Clave', puedes pasar 'NuevaClave' 
                    // directamente como un parámetro extra a tu capa de negocio.
                    modelo.Clave = NuevaClave;
                }

                // Llamamos a la capa de negocio enviando el modelo y la nueva clave opcional
                bool respuesta = _usuariosNegocio.MtdActualizarUsuario(modelo, NuevaClave);

                if (respuesta)
                {
                    // Si todo salió bien, vuelve a la lista general de usuarios
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "No se pudieron actualizar los datos del usuario en la base de datos.";
                }
            }
            catch (Exception ex)
            {
                // Si hay un error de SQL (como que falte el parámetro en el SP), saltará aquí y te dirá qué pasa
                ViewBag.Error = $"Error técnico al editar: {ex.Message}";
            }

            // Si hubo un error, vuelve a cargar la vista con los datos que ya tenía
            return View(modelo);
        }
            }

            // ── ELIMINAR ──
            [Authorize(Roles = "1")]
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
