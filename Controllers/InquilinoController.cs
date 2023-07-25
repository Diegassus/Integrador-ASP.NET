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
        public InquilinoController()
        {
            Repo = new RepositorioInquilino();
        }

        // GET: Inquilino
        public ActionResult Index()
        {
            var inquilinos = Repo.ObtenerInquilinos();
            if(TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Exito = 1;
                ViewBag.Mensaje = TempData["Mensaje"];
            }
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
                if(ModelState.IsValid)
                {
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se registro a " + inquilino.Nombre + " " + inquilino.Apellido + " correctamente";
                    var res = Repo.CrearInquilino(inquilino);
                    return View();
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo registrar al inquilino";
                    return View();
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
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
                if(ModelState.IsValid)
                {
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se actualizó correctamente a " + inquilino.Nombre + " " + inquilino.Apellido;
                    inquilino.Id = id ;
                    Repo.EditarInquilino(inquilino);
                    inquilino = Repo.ObtenerInquilino(id);
                    return View(inquilino);
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo actualizar al inquilino";
                    inquilino = Repo.ObtenerInquilino(id);
                    return View(inquilino);
                }
                
            }
            catch
            {
                return RedirectToAction(nameof(Index));
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
                var res = Repo.EliminarInquilino(id);
                TempData["Mensaje"] = "Se elimino correctamente al inquilino";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "No se pudo eliminar al inquilino " + ex;
                return View(inquilino);
            }
        }
    }
}