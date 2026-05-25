using Sistema_Financiero.data;
using Sistema_Financiero.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ConexionDatos>();
builder.Services.AddScoped<EmpleadosDatos>();
builder.Services.AddScoped<EmpleadosNegocio>();
builder.Services.AddScoped<UsuariosNegocio>();
builder.Services.AddScoped<UsuariosDatos>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Si intentan entrar sin permiso, los manda aquí
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Tiempo de inactividad antes de cerrarse
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();


app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
