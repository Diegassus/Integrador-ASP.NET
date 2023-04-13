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
    public class PagoController : Controller
    {
        RepositorioPago Repo ;

        public PagoController(){
            Repo = new RepositorioPago();
        }
        // GET: Pago
        public ActionResult Index(int id)
        {
            var res = Repo.ObtenerPagos(id);
            ViewBag.Contrato = id;
            return View(res);
        }

        // GET: Pago/Details/5
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerPago(id);
            ViewBag.Contrato = res.ContratoId;
            return View(res);
        }

        // GET: Pago/Create
        public ActionResult Create(int id)
        {
            ViewBag.Contrato = id;
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                // TODO: Add insert logic here
                var res = Repo.CrearPago(pago);
                return RedirectToAction("Index", new {id = res});
            }
            catch
            {
                throw;
            }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            var res = Repo.ObtenerPago(id);
            ViewBag.Contrato = res.ContratoId;
            return View(res);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                // TODO: Add update logic here
                p.Id=id;
                var res = Repo.EditarPago(p);
                return RedirectToAction("Index", new {id = p.ContratoId}); // arreglar redirect
            }
            catch
            {
               throw;
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerPago(id);
            ViewBag.Contrato = res.ContratoId;
            return View(res);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                // TODO: Add delete logic here
                pago = Repo.ObtenerPago(id);
                var res = Repo.EliminarPago(id);
                return RedirectToAction("Index", new {id = pago.ContratoId});
            }
            catch
            {
                throw;
            }
        }
    }
}