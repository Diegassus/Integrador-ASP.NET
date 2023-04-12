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
    public class ContratosController : Controller
    {
        RepositorioContrato Repo ;
        RepositorioInmueble RepoInmueble;
        RepositorioInquilino RepoInquilino;
        public ContratosController(){
            Repo = new RepositorioContrato();
            RepoInmueble = new RepositorioInmueble();
            RepoInquilino = new RepositorioInquilino();
        }

        // GET: Contratos
        public ActionResult Index()
        {
            var res = Repo.ObtenerContratos();
            ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
            return View(res);
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerContrato(id);
            return View(res);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                // TODO: Add insert logic here
                var res = Repo.CrearContrato(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                throw;
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            var res = Repo.ObtenerContrato(id);
            return View(res);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add update logic here
                contrato.Id = id ;
                var res = Repo.EditarContrato(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerContrato(id);
            return View();
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                // TODO: Add delete logic here
                var res = Repo.EliminarContrato(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}