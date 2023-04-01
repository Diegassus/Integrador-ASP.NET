

using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Repositorios;

public class RepositorioContrato{
    
    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";
    public RepositorioContrato(){

    }

    public int EliminarContrato(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM contratos WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res ;
    }

    public int EditarContrato(Contrato c)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {//Lat, Lng, Tipo, Uso, Ambientes, Disponible, Direccion, Precio, PropietarioId
            var query = @"UPDATE contratos SET
            Desde = @desde,
            Hasta = @hasta,
            Estado = @estado,
            Mensualidad = @mensualidad,
            InmuebleId = @inmuebleId,
            InquilinoId = @inquilinoId
            WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@desde",c.Desde);
                command.Parameters.AddWithValue("@hasta",c.Hasta);
                command.Parameters.AddWithValue("@estado",c.Estado);
                command.Parameters.AddWithValue("@mensualidad",c.Mensualidad);
                command.Parameters.AddWithValue("@inmuebleId",c.InmuebleId);
                command.Parameters.AddWithValue("@inquilinoId",c.InquilinoId);
                command.Parameters.AddWithValue("@id",c.Id);
                connection.Open();
                res = command.ExecuteNonQuery() ;
                connection.Close();
            }
        }
        return res ;
    }

    public int CrearContrato(Contrato c)
    {
        var res = -1 ;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO contratos 
            (Desde, Hasta, Estado, Mensualidad, InmuebleId, InquilinoId) 
            VALUES (@desde, @hasta, @estado, @mensualidad, @inmuebleId, @inquilinoId);
            SELECT LAST_INSERT_ID()"; // devuelve el id insertado
            using( var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@desde",c.Desde);
                command.Parameters.AddWithValue("@hasta",c.Hasta);
                command.Parameters.AddWithValue("@estado",true);
                command.Parameters.AddWithValue("@mensualidad",c.Mensualidad);
                command.Parameters.AddWithValue("@inmuebleId",c.InmuebleId);
                command.Parameters.AddWithValue("@inquilinoId",c.InquilinoId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res ;
    }

    public Contrato ObtenerContrato(int id)
    {
        Contrato res = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT c.Id, c.Desde, c.Hasta, c.Estado, c.Mensualidad, c.InmuebleId, c.InquilinoId,
            inmuebles.Id , Lat, Lng, Tipo, Uso, Ambientes, Direccion, Precio, Disponible, PropietarioId,
            inquilinos.Id, Nombre, Apellido,Dni, Telefono,Correo, inquilinos.Estado 
            FROM contratos c 
            INNER JOIN inmuebles 
            ON inmuebles.Id = c.InmuebleId
            INNER JOIN inquilinos 
            ON inquilinos.Id = c.InquilinoId
            WHERE c.Id = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            Desde = reader.GetDateTime(nameof(Contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(Contrato.Hasta)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Mensualidad = reader.GetDecimal(nameof(Contrato.Mensualidad)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            Bien = new Inmueble{
                                Id = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Lat = reader.GetString(nameof(Inmueble.Lat)),
                                Lng = reader.GetString(nameof(Inmueble.Lng)),
                                Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                                Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                                Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                                Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            },
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Arrendatario = new Inquilino{
                                Id = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Correo = reader.GetString(nameof(Propietario.Correo)),
                                Estado = reader.GetBoolean(nameof(Propietario.Estado))
                            }
                            
                        };
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Contrato> ObtenerContratos()
    {
        List<Contrato> res = new List<Contrato>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id, Desde, Hasta, Estado, Mensualidad, InmuebleId, InquilinoId
            FROM contratos ";
            using( var command = new MySqlCommand( query , connection )) 
            {
                connection.Open();
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            Desde = reader.GetDateTime(nameof(Contrato.Desde)),
                            Hasta = reader.GetDateTime(nameof(Contrato.Hasta)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Mensualidad = reader.GetDecimal(nameof(Contrato.Mensualidad)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId))
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }


}