using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Repositorios;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario Repo;
        // GET: Propietario
        public PropietarioController()
        {
            Repo = new RepositorioPropietario();
        }
        public ActionResult Index()
        {
            var res = Repo.ObtenerPropietarios();
            return View(res);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            return View(propietario);
        }

        // GET: Propietario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                // TODO: Add insert logic here
                var res = Repo.CrearPropietario(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                // TODO: Add update logic here
                propietario.Id = id ;
                Repo.EditarPropietario(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            return View(propietario);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Repo.EliminarPropietario(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}