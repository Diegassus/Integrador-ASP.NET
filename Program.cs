using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
  options =>
  {
    options.LoginPath = "/Usuarios/login";
    options.LogoutPath = "/Usuarios/logout";
    options.AccessDeniedPath = "/Home/Privacy";
  }  
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Empleado",policy => policy.RequireRole("Empleado","Administrador"));
    options.AddPolicy("Administrador",policy => policy.RequireRole("Administrador"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/*
Para la última entrega parcial (antes de la presentación y defensa), se pide:
    
    Actalizacion de claim cuando se modifica un ususario
    Ocultar opciones de delete para empleados comunes, emprolijar vistas y textos, revisar validaciones de los datos ingresados y cargas optimas a la BD , validar el cambio de contraseña.
    Notificaciones de error, advetrencia y exito
    validacion y confirmacion de cambio de contraseña

Tener en cuenta estilos, textos, ortografía y presentación en general.

Se debe entregar:

    Diagrama de entidad-relación o de clases. Incluir en el repositorio.
    Repositorio git con el proyecto. Debe ser público. Incluir gitignore para que no se incluyan los archivos compilados (bin y obj).
    Base de datos actualizada. Incluir script en el repositorio.
    Incluir en esta entrega, usuario y contraseña de un administrador y de un empleado.


*/