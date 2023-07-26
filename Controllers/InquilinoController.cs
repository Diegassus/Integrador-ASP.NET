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
            ViewBag.Exito = TempData["Exito"];
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(inquilinos);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            if(inquilino == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inquilino solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                    var res = Repo.CrearInquilino(inquilino);
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se registro a " + inquilino.Nombre + " " + inquilino.Apellido + " correctamente";
                    return View();
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo registrar al inquilino";
                    return View();
                }
            }
            catch
            {
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "Ya existe un inquilino registrado con este correo";
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            if(inquilino == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inquilino solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                    inquilino.Id = id ;
                    Repo.EditarInquilino(inquilino);
                    inquilino = Repo.ObtenerInquilino(id);
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se actualizó correctamente a " + inquilino.Nombre + " " + inquilino.Apellido;
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
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "Ocurrio un problema al editar el inquilino";
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var inquilino = Repo.ObtenerInquilino(id);
            if(inquilino == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inquilino solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                if(ModelState.IsValid){
                    var res = Repo.EliminarInquilino(id);
                    TempData["Exito"] = 1;
                    TempData["Mensaje"] = "Se elimino correctamente al inquilino";
                    return RedirectToAction(nameof(Index));
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo eliiminar al inquilino";
                    return View(inquilino);
                }
            }
            catch
            {
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un problema al intentar eliminar el inquilino";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}