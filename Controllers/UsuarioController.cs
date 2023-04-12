using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private RepositorioUsuario Repo;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public UsuarioController(IConfiguration configuration, IWebHostEnvironment enviroment){
            Repo = new RepositorioUsuario();
            this.configuration = configuration;
            this.environment = enviroment ;
        }
        // GET: Usuario
        [Authorize(Policy ="Administrador")]
        public ActionResult Index()
        {
            var res = Repo.ObtenerUsuarios();
            return View(res);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy ="Administrador")]
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerPorId(id);
            return View(res);
        }

        // GET: Usuario/Create
        [Authorize(Policy ="Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy ="Administrador")]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                // TODO: Add insert logic here
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password : usuario.Clave,
                        salt : System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf : KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));
                usuario.Clave = hashed ;
                //usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado ;
                var res = Repo.CrearUsuario(usuario);
                if(usuario.AvatarFile != null && usuario.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath,"Uploads");
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path,fileName);
                    usuario.Avatar = Path.Combine("/Uploads",fileName);
                    using (FileStream stream = new FileStream(pathCompleto,FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                    Repo.EditarUsuario(usuario);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult Perfil()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            ViewBag.Titulo = "Mi Perfil";
            var u = Repo.ObtenerPorCorreo(User.Identity.Name);
            return View("Edit",u);
        }

        // GET: Usuario/Edit/5
        [Authorize(Policy ="Administrador")]
        public ActionResult Edit(int id)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            ViewBag.Titulo = "Editar" ;
            var res = Repo.ObtenerPorId(id);
            return View(res);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id , Usuario u)
        {
            var us = Repo.ObtenerPorId(id);
            //var vista = nameof(Edit);
            try
            {
                if(u.Clave == null || u.Clave == "")
                {
                    u.Clave = us.Clave ;
                }else{
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password : u.Clave,
                            salt : System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf : KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8
                        ));
                    u.Clave = hashed ;
                }

                if(u.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath,"Uploads");
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path,fileName);
                    u.Avatar = Path.Combine("/Uploads",fileName);
                    using (FileStream stream = new FileStream(pathCompleto,FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                }else{
                    u.Avatar = us.Avatar;
                }

                if(!User.IsInRole("Administrador"))
                {
                    //vista = nameof(Perfil);
                    var usuarioActual = Repo.ObtenerPorCorreo(User.Identity.Name);
                    if(usuarioActual.Id != id)
                    {
                        return RedirectToAction(nameof(Index),"Home");
                    }
                }
                u.Id = id ;
                var res = Repo.EditarUsuario(u);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return RedirectToAction(nameof(Index),"Home");
            }catch
            {
                throw;
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy ="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerPorId(id);
            return View(res);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy ="Administrador")]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                // TODO: Add delete logic here
                var res = Repo.EliminarUsuario(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET Usuario/login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST Usuario/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password : login.Clave,
                        salt : System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf : KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));
                    var r = Repo.ObtenerPorCorreo(login.Usuario);
                    if(r == null || r.Clave != hashed)
                    {
                        ModelState.AddModelError("","Usuario o contraseña incorrecto");
                        return View();
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, r.Correo),
                        new Claim(ClaimTypes.Role, r.RolNombre)
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity));
                    return Redirect("/Home");
                }
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            return RedirectToAction("Index","Home");
        }
    }
}