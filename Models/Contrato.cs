using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models;

public class Contrato {
    [Display(Name ="Codigo")]
    public int Id { get ; set ; }
    public DateTime Desde { get ; set ; }
    public DateTime Hasta { get ; set ; }
    public bool Estado { get ; set ; }
    public decimal Mensualidad { get ; set ; }
    [Display(Name ="Codigo de inmueble")]
    public int InmuebleId { get ; set ; }
    [ForeignKey(nameof(Inmueble.Id))]
    public Inmueble Bien { get ; set ;}
    [Display(Name ="Inquilino")]
    public int InquilinoId { get ; set ; }
    [ForeignKey(nameof(Inquilino.Id))]
    public Inquilino Arrendatario { get ; set ; }
}