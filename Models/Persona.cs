using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models ;

public class Persona {
    [Display(Name = "Codigo")]
    public int Id { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public bool Estado { get; set; }

    public Persona()
    {
        
    }

    
    public string ToString(){
        return Nombre + " " + Apellido ;
    }
}