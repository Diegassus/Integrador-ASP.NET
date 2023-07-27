using Microsoft.AspNetCore.Authentication.Cookies;

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

/*  Rediseñar las vistas para que esten mas presentables (revisar rutas y contorladores de la carpeta de listados [Vistas] agregarle enlace para ir al detalle del dueño de la propiedad a todas las vistas donde se vea un inmueble)

    validar que exista el inquilino /  usuario siempre en cada ruta necesaria y que no se puedan repetir datos que deberin ser unicos

    [Vista Inmuebles > listar por disponibilidad | listar contratos del inmueble]
    Listar todos los contratos de un inmueble en particular.

    [Vista Contrato > listar por fechas]
    Listar todos los contratos de alquiler que se encuentren vigentes (por fechas desde/hasta).

    [Vista crear contrato (rediseño y reestructurar)]
    Dadas dos fechas posibles de un contrato (inicio y fin), listar todos los inmuebles que no estén ocupados en algún contrato entre esas fechas.

    Controlar que no existe super posición de fechas de contratos al crear/editar contratos.
    Permitir renovar contratos. (Crea un contrato nuevo)
    Permitir terminar tempranamente contratos indicando la multa. (mostrar un toast)

    Usabilidad para el usuario, como menú de navegación, listados con datos representativos, nombres de campos apropiados, ortografía, accesos directos, notificaciones (resultado de operaciones y errores entre otras), validaciones de datos, etc.
    Rehacer controladores y metodos que esten trabajando deficientemente (Como no hacer join o no usar bien los modelos)

Entregar:

    Diagrama completo
    Base de datos
    Repositorio git
    Usuario y claves para administrador y otro usuario no administrador

*/

/*
Completado

    [Vista propietario > listar inmuebles de un popietario]
    Listar todos los inmuebles que le correspondan a un propietario.

    [Vista Inmuebles > listar por disponibilidad | listar contratos del inmueble]
    Listar los inmuebles que estén disponibles (estado disponible, no por fechas) y su dueño.

*/