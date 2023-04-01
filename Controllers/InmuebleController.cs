using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria.Repositorios;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private RepositorioInmueble Repo;
        private RepositorioPropietario RepoPropietarios;
        public InmuebleController(){
            Repo = new RepositorioInmueble();
            RepoPropietarios = new RepositorioPropietario();
        }

        // GET: Inmueble
        public ActionResult Index()
        {
            var inmuebles = Repo.ObtenerInmuebles();
            return View(inmuebles);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            return View(res);
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            try{
                ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                return View();
            }catch(Exception e)
            {
                throw ;
            }
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
                {
                    // TODO: Add insert logic here
                    Repo.CrearInmueble(inmueble);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e)
                {
                    throw e ;
                }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            return View(res);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                // TODO: Add update logic here
                i.Id = id ;
                Repo.EditarInmueble(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            return View(res);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                // TODO: Add delete logic here
                Repo.EliminarInmueble(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}