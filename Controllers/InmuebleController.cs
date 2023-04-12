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
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var inmuebles = Repo.ObtenerInmuebles();

            return View(inmuebles);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            res.Duenio = RepoPropietarios.ObtenerPropietario(res.PropietarioId);
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            return View(res);
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            try{
                ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                return View();
            }catch
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
            ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
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
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            return View(res);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
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