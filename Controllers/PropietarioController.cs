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
        public PropietarioController()
        {
            Repo = new RepositorioPropietario();
        }

        // GET: Propietario
        public ActionResult Index()
        {
            var res = Repo.ObtenerPropietarios();
            ViewBag.Exito = TempData["Exito"];
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(res);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            if(propietario == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el propietario solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                if(ModelState.IsValid){
                    var res = Repo.CrearPropietario(propietario);
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se registro a " + propietario.Nombre + " " + propietario.Apellido + " correctamente";
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
                ViewBag.Mensaje = "Ya existe un propietario registrado con este correo";
                return View();
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            if(propietario == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el propietario solicitado";
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se actualizó correctamente a " + propietario.Nombre + " " + propietario.Apellido;
                    propietario.Id = id ;
                    Repo.EditarPropietario(propietario);
                    return View(propietario);
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo actualizar al propietario";
                    propietario = Repo.ObtenerPropietario(id);
                    return View(propietario);
                }
            }
            catch
            {
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "Ocurrio un problema al editar el propietario";
                return View();
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var propietario = Repo.ObtenerPropietario(id);
            if(propietario == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el propietario solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                if(ModelState.IsValid)
                {
                    TempData["Exito"] = 1;
                    TempData["Mensaje"] = "Se elimino correctamente el propietario";
                    Repo.EliminarPropietario(id);
                    return RedirectToAction(nameof(Index));
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo eliiminar al propietario";
                    var propietario = Repo.ObtenerPropietario(id);
                    return View(propietario);
                }
            }
            catch
            {
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un problema al intentar eliminar el propietario";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}