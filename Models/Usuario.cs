

using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;

public class Usuario : Persona {
    public string? Clave { get ; set ; }
    public string? Avatar { get ; set ; }
    public IFormFile AvatarFile { get ; set ; }
    [Display(Name ="Rol del empleado")]
    public int? Rol { get ; set ; }
    public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "" ;

    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(enRoles);
        foreach(var value in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)value,Enum.GetName(tipoEnumRol,value));
        }
        return roles ;
    }
}

public enum enRoles
{
    Empleado = 1 ,
    Administrador = 2 
}