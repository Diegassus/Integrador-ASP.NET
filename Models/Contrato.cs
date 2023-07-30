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
    public int InmuebleId { get ; set ; }
    [ForeignKey(nameof(Inmueble.Id))]
    [Display(Name ="Inmueble")]
    public Inmueble? Bien { get ; set ;}
    public int InquilinoId { get ; set ; }
    [ForeignKey(nameof(Inquilino.Id))]
    [Display(Name ="Inquilino")]
    public Inquilino? Arrendatario { get ; set ; }
}