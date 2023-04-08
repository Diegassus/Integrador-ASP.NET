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
    public class UsuarioController : Controller
    {
        private RepositorioUsuario Repo;
        public UsuarioController(){
            Repo = new RepositorioUsuario();
        }
        // GET: Usuario
        public ActionResult Index()
        {
            var res = Repo.ObtenerUsuarios();
            return View(res);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

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
                        salt : System.Text.Encoding.ASCII.GetBytes("lacon-trasenia"),
                        prf : KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));
                    var r = Repo.ObtenerPorCorreo(login.Usuario);
                    if(r == null || r.Clave != hashed)
                    {
                        ModelState.AddModelError("","El email o la clave son incorrectos");
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
    }
}