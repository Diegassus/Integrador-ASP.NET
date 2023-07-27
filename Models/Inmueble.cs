

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
        [Display(Name ="Estado")]
        public bool Disponible { get ; set ;}
        public string? Direccion { get ; set ; }
        public decimal Precio { get ; set ; }
        public int PropietarioId { get ; set ; }
        [ForeignKey(nameof(PropietarioId))]
        [Display(Name ="Propietario")]
        public Propietario? Duenio {get ; set ;}


        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnumUso = typeof(enUso);
            foreach(var value in Enum.GetValues(tipoEnumUso))
            {
                usos.Add((int)value,Enum.GetName(tipoEnumUso,value));
            }
            return usos ;
        }

        public static IDictionary<int, string> ObtenerTipos()
            {
                SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
                Type tipoEnumTipo = typeof(enTipo);
                foreach(var value in Enum.GetValues(tipoEnumTipo))
                {
                    roles.Add((int)value,Enum.GetName(tipoEnumTipo,value));
                }
                return roles ;
            }
    }

    
}

public enum enUso
{
    comercial = 1,
    temporal = 2,
    permanente = 3
}

public enum enTipo
{
    departamento = 1,
    casa = 2,
    comercio = 3,
    galpon = 4
}