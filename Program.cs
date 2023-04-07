using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/*
Para la última entrega parcial (antes de la presentación y defensa), se pide:
    
    Debe existir un ABM de Usuarios del sistema. Se debe distinguir entre los roles de administradores y empleados. Solo los administradores pueden gestionar otros usuarios (ABM).
    Todos los ABM deben estar restringidos a usuarios logueados en el sistema. Solo los administradores pueden eliminar.
    Cada usuario puede modificar su perfil sin modificar el rol. Incluye cambio de avatar y de contraseña.
    Actualizar el menú de navegación y las acciones disponibles de acuerdo al rol del usuario logueado.

Tener en cuenta estilos, textos, ortografía y presentación en general.

Se debe entregar:

    Diagrama de entidad-relación o de clases. Incluir en el repositorio.
    Repositorio git con el proyecto. Debe ser público. Incluir gitignore para que no se incluyan los archivos compilados (bin y obj).
    Base de datos actualizada. Incluir script en el repositorio.
    Incluir en esta entrega, usuario y contraseña de un administrador y de un empleado.


*/