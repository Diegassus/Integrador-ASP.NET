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
            ViewBag.Exito = TempData["Exito"];
            ViewBag.Mensaje = TempData["Mensaje"];
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
            return View();
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if(ModelState.IsValid){
                    if(contrato.Desde == null || contrato.Hasta == null){
                        return RedirectToAction(nameof(Index));
                    }
                    if(contrato.Hasta < contrato.Desde){
                        return RedirectToAction(nameof(Index));
                    }
                    if(Repo.ValidarFecha(contrato)){
                        Repo.CrearContrato(contrato);
                        ViewBag.Exito = 1;
                        ViewBag.Mensaje = "Contrato creado correctamente";
                        return View();
                    }else{
                        ViewBag.Exito = 0;
                        ViewBag.Mensaje = "Ya existe un contrato con este inmueble, para las fechas seleccionadas";
                        return View();
                    }
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "Ocurrio un problema al crear el contrato";
                    return View();
                }
            }
            catch
            {
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un problema al crear el contrato";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            var res = Repo.ObtenerContrato(id);
            ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
            return View(res);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                if(ModelState.IsValid){
                    if(contrato.Desde == null || contrato.Hasta == null){
                        return RedirectToAction(nameof(Index));
                    }
                    if(contrato.Hasta < contrato.Desde){
                        return RedirectToAction(nameof(Index));
                    }
                    if(Repo.ValidarFecha(contrato)){
                        Repo.EditarContrato(contrato);
                        ViewBag.Exito = 1;
                        ViewBag.Mensaje = "Contrato editado correctamente";
                        ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
                        ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
                        return View(contrato);
                    }else{
                        ViewBag.Exito = 0;
                        ViewBag.Mensaje = "Este inmueble esta ocupado durante esas fechas";
                        ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
                        ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
                        return View(contrato);
                    }
                }
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "Ocurrio un problema al editar el contrato";
                ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
                ViewBag.Inmuebles = RepoInmueble.ObtenerInmuebles();
                return View(contrato);
            }
            catch
            {
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un problema al editar el contrato";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerContrato(id);
            return View(res);
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

        // GET: Contratos/Vigentes
        public ActionResult Vigentes(){
            var contratos = Repo.ObtenerVigentes();
            return View(contratos);
        }

        // GET: Contratos/Propiedad/:id
        public ActionResult Propiedad(int id){
            var contratos = Repo.ContratosPropiedad(id);
            return View(contratos);
        }

        // POST: contratos/filtro
        [HttpPost]
        public ActionResult Filtro(Filtro filtro){
            try{
                if(ModelState.IsValid){
                    if(filtro.Desde == null || filtro.Hasta == null){
                        return RedirectToAction(nameof(Vigentes));
                    }
                    if(filtro.Hasta < filtro.Desde){
                        return RedirectToAction(nameof(Vigentes));
                    }
                    var contratos = Repo.FiltrarContratos(filtro);
                    return View(contratos);
                }else{
                    return RedirectToAction(nameof(Vigentes));
                }
            }catch{
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un error al filtrar los contratos";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: contratos/nuevo
        [HttpPost]
        public ActionResult Nuevo(Filtro filtro){
            try{
                if(ModelState.IsValid){
                    if(filtro.Desde == null || filtro.Hasta == null){
                        return RedirectToAction(nameof(Create));
                    }
                    if(filtro.Hasta < filtro.Desde){
                        return RedirectToAction(nameof(Create));
                    }
                    ViewBag.Desde = filtro.Desde;
                    ViewBag.Hasta = filtro.Hasta;
                    ViewBag.Inquilinos = RepoInquilino.ObtenerInquilinos();
                    ViewBag.Inmuebles = Repo.InmueblesDisponiblesPorFecha(filtro);
                    return View();
                }else{
                    return RedirectToAction(nameof(Create));
                }
            }catch{
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un error al obtener las propiedades";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: contratos/renovar
        public ActionResult Renovar(int id){
            try
            {
                var contrato = Repo.ObtenerContrato(id);
                if(contrato.Desde == null || contrato.Hasta == null){
                    return RedirectToAction(nameof(Index));
                }
                if(contrato.Hasta < contrato.Desde){
                    return RedirectToAction(nameof(Index));
                }
                contrato.Desde = contrato.Hasta.AddDays(1);
                contrato.Hasta = contrato.Desde.AddYears(1);
                if(Repo.ValidarFecha(contrato)){
                    Repo.CrearContrato(contrato);
                    TempData["Exito"] = 1;
                    TempData["Mensaje"] = "Contrato renovado correctamente";
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["Exito"] = 0;
                    TempData["Mensaje"] = "Este inmueble esta ocupado durante esas fechas";
                    return RedirectToAction(nameof(Index));
                }
            }catch{
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un error al renovar el contrato";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: contratos/cancelar
        public ActionResult Cancelar(int id){
            try
            {
                var contrato = Repo.ObtenerContrato(id);
                if(contrato == null){
                    TempData["Exito"] = 0;
                    TempData["Mensaje"] = "No existe el contrato seleccionado";
                    return RedirectToAction(nameof(Index));
                }
                Repo.Cancelar(contrato);
                TempData["Exito"] = 1;
                TempData["Mensaje"] = "Contrato cancelado, el inquilino debe pagar una multa de $" + contrato.Mensualidad*6;
                return RedirectToAction(nameof(Index));
            }catch{
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "Ocurrio un error al cancelar el contrato";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}