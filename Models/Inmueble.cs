

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{

    public class Inmueble{
        [Display(Name = "Código")]
        public int Id { get ; set ;}
        [Display(Name ="Latitud")]
        public string? Lat { get ; set ; }
        [Display(Name ="Longitud")]
        public string? Lng { get ; set ; }
        public int? Uso { get ; set ; }
        public int? Tipo { get ; set ; }
        public int? Ambientes { get ; set ; }
        public bool? Disponible { get ; set ;}
        public string? Direccion { get ; set ; }
        public double? Precio { get ; set ; }
        public int PropietarioId { get ; set ; }
        [ForeignKey(nameof(PropietarioId))]
        public Propietario Duenio {get ; set ;}
    }
}