using Inmobiliaria.Models;
using Inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class ListadosController : Controller
    {
        private readonly RepositorioPropietario RPropietario;
        private readonly RepositorioInmueble RInmueble;
        private readonly RepositorioContrato RContrato;
        private readonly RepositorioInquilino RInquilino;
        public ListadosController(){
            RPropietario = new RepositorioPropietario();
            RInmueble = new RepositorioInmueble();
            RContrato = new RepositorioContrato();
            RInquilino = new RepositorioInquilino();
        }

        // GET: Inmuebles de un Propietario
        public ActionResult Index(int id)
        {
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var res = RInmueble.InmueblesPropietario(id);
            return View(res);
        }

        // GET: Inmuebles disponibles
        public ActionResult Disponibles()
        {
            ViewBag.Usos = Inmueble.ObtenerUsos();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var res = RInmueble.InmueblesDisponibles();
            return View(res);
        }

        // GET: Todos los contratos de una propiedad
        public ActionResult Contratos(int id)
        {
            ViewBag.Inquilinos = RInquilino.ObtenerInquilinos();
            ViewBag.Inmuebles = RInmueble.ObtenerInmuebles();
            var res = RContrato.ContratosPropiedad(id);
            return View(res);
        }
    }
}