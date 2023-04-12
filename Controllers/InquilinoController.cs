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
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino Repo;
        // GET: Inquilino
        public InquilinoController()
        {
            Repo = new RepositorioInquilino();
        }
        public ActionResult Index()
        {
            var inquilinos = Repo.ObtenerInquilinos();
            return View(inquilinos);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            return View(inquilino);
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                var res = Repo.CrearInquilino(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                // TODO: Add update logic here
                inquilino.Id = id ;
                Repo.EditarInquilino(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            return View(inquilino);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                // TODO: Add delete logic here
                Repo.EliminarInquilino(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}