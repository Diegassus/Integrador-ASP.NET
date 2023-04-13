using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models;

public class Pago{

    [Display(Name="Codigo")]
    public int Id { get ; set ;}
    [Display(Name ="Numero de pago")]
    public int Nro { get ; set;}
    public DateTime Fecha { get ; set ; }
    public Decimal Importe { get ; set ; }
    [Display(Name ="Codigo de contrato")]
    public int ContratoId { get ; set ; }
    [ForeignKey(nameof(ContratoId))]
    public Contrato? Contrato { get ; set ; }

}