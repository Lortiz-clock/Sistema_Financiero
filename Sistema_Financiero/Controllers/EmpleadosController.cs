
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_Financiero.Services;


namespace Sistema_Financiero.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadosNegocio _empleadosNegocio;
        
        public EmpleadosController(EmpleadosNegocio empleadosNegocio)
        {
            _empleadosNegocio = empleadosNegocio;
        }
        public IActionResult Index()
        {
            var empleados = _empleadosNegocio.MtdConsultarEmpleados();
            return View(empleados);
        }
    }
}
