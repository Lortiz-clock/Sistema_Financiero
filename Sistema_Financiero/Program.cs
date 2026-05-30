using Sistema_Financiero.data;
using Sistema_Financiero.Services;
using Sistema_Financiero.Logica;
using Sistema_Financiero.Models;

var builder = WebApplication.CreateBuilder(args);

// ── 1. CONFIGURACIÓN DEL SISTEMA DE VISTAS (Unificado) ──
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// ── 2. REGISTRO DE CAPAS DE DATOS (Data Access) ──
builder.Services.AddScoped<ConexionDatos>();
builder.Services.AddScoped<ClientesDatos>();
builder.Services.AddScoped<MunicipioDatos>(); // ◄ Inyectado con éxito para solucionar la excepción
builder.Services.AddScoped<EmpleadosDatos>();
builder.Services.AddScoped<UsuariosDatos>();
builder.Services.AddScoped<SucursalesDatos>();
builder.Services.AddTransient<RegionDatos>();

// ── 3. REGISTRO DE CAPAS DE NEGOCIO (Services) ──
builder.Services.AddScoped<ClientesNegocio>();
builder.Services.AddScoped<EmpleadosNegocio>();
builder.Services.AddScoped<UsuariosNegocio>();
builder.Services.AddScoped<SucursalesNegocio>();
builder.Services.AddTransient<RegionNegocio>(); // ◄ Centralizado (se eliminó el duplicado Scoped de abajo)

// ── 4. CONFIGURACIÓN DE AUTENTICACIÓN POR COOKIES ──
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";               // Redirección si no está autenticado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);  // Tiempo de inactividad de la sesión
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirección por falta de rol/permiso
    });

var app = builder.Build();

// ── 5. CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP ──
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

// Control de seguridad: Autenticación y Autorización
app.UseAuthentication(); // ◄ ¡REVISE AQUÍ! Agregado para que el sistema valide las cookies antes de evaluar los Roles
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();