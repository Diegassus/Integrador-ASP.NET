using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Inmobiliaria.Models;

public class Filtro {
    public Filtro(DateTime desde, DateTime hasta)
    {
        Desde = desde;
        Hasta = hasta;
    }

    public DateTime Desde { get ; set ; }
    public DateTime Hasta { get ; set ; }
}