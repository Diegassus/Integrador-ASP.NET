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
            var inmuebles = Repo.ObtenerInmuebles();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            ViewBag.Exito = TempData["Exito"];
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(inmuebles);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            if(res == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inmueble solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                    if(ModelState.IsValid){
                        Repo.CrearInmueble(inmueble);
                        ViewBag.Exito = 1;
                        ViewBag.Mensaje = "Se registro correctamente el inmueble";
                        ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                        ViewBag.Usos = Inmueble.ObtenerUsos();
                        ViewBag.Tipos = Inmueble.ObtenerTipos();
                        return View();
                    }else{
                        ViewBag.Exito = 0;
                        ViewBag.Mensaje = "No se pudo registrar el inmueble";
                        ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                        ViewBag.Usos = Inmueble.ObtenerUsos();
                        ViewBag.Tipos = Inmueble.ObtenerTipos();
                        return View();
                    }
                }
                catch
                {
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "Ya existe un inmueble declarado en esa direccion";
                    ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    return View();
                }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            if(res == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inmueble solicitado";
                return RedirectToAction(nameof(Index));
            }
            ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            return View(res);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                if(ModelState.IsValid){
                    inmueble.Id = id ;
                    Repo.EditarInmueble(inmueble);
                    inmueble = Repo.ObtenerInmueble(id);
                    ViewBag.Exito = 1;
                    ViewBag.Mensaje = "Se edito correctamente el inmueble";
                    ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo editar el inmueble";
                    ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                }
                return View(inmueble);
            }
            catch
            {
                ViewBag.Exito = 0;
                ViewBag.Mensaje = "Ocurrio un problema el intentar editar el inmueble";
                ViewData["propietarios"] = RepoPropietarios.ObtenerPropietarios();
                ViewBag.Usos = Inmueble.ObtenerUsos();
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            var res = Repo.ObtenerInmueble(id);
            if(res == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el inmueble solicitado";
                return RedirectToAction(nameof(Index));
            }
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
                if(ModelState.IsValid){
                    Repo.EliminarInmueble(id);
                    TempData["Exito"] = 1;
                    TempData["Mensaje"] = "Se elimino correctamente el inmueble";
                    return RedirectToAction(nameof(Index));
                }else{
                    ViewBag.Exito = 0;
                    ViewBag.Mensaje = "No se pudo eliminar el inmueble";
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    return View(inmueble);
                }
            }
            catch
            {
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No se pudo eliminar el inmueble";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmuebles disponibles
        public ActionResult Disponibles()
        {
            var inmuebles = Repo.InmueblesDisponibles();
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            return View(inmuebles);
        }

        // GET: Inmuebles de un propietario
        public ActionResult Pertenecen(int id){
            var inmuebles = Repo.InmueblesPropietario(id);
            if(inmuebles == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existen inmuebles de este propietario";
                return RedirectToAction(nameof(Index), nameof(PropietarioController));
            }
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var propietario = RepoPropietarios.ObtenerPropietario(id);
            if(propietario == null){
                TempData["Exito"] = 0;
                TempData["Mensaje"] = "No existe el propietario seleccionado";
                return RedirectToAction(nameof(Index), nameof(PropietarioController));
            }
            ViewBag.Propietario = propietario.Nombre + " " + propietario.Apellido;
            return View(inmuebles);
        }
    }
}