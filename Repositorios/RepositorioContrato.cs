

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
            var query = @"SELECT contratos.Id, Desde, Hasta, contratos.Estado, Mensualidad, InmuebleId, InquilinoId , inmuebles.Direccion , inquilinos.Nombre , inquilinos.Apellido
            FROM contratos 
            JOIN inmuebles ON inmuebles.Id = contratos.InmuebleId 
            JOIN inquilinos ON inquilinos.Id = contratos.InquilinoId";
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
                            Bien = new Inmueble{
                                Id = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion))
                            },
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Arrendatario = new Inquilino{
                                Id = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Contrato> ObtenerVigentes()
    {
        List<Contrato> res = new List<Contrato>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT contratos.Id, Desde, Hasta, contratos.Estado, Mensualidad, InmuebleId, InquilinoId , inmuebles.Direccion , inquilinos.Nombre , inquilinos.Apellido
            FROM contratos 
            JOIN inmuebles ON inmuebles.Id = contratos.InmuebleId 
            JOIN inquilinos ON inquilinos.Id = contratos.InquilinoId
            WHERE contratos.Estado = 1";
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
                            Bien = new Inmueble{
                                Id = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion))
                            },
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Arrendatario = new Inquilino{
                                Id = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Contrato> ContratosPropiedad(int id){
        List<Contrato> res = new List<Contrato>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT contratos.Id, Desde, Hasta, contratos.Estado, Mensualidad, InmuebleId, InquilinoId , inmuebles.Direccion , inquilinos.Nombre , inquilinos.Apellido
            FROM contratos 
            JOIN inmuebles ON inmuebles.Id = contratos.InmuebleId 
            JOIN inquilinos ON inquilinos.Id = contratos.InquilinoId
            WHERE inmuebles.Id = @id";
            using( var command = new MySqlCommand( query , connection )) 
            {
                connection.Open();
                command.Parameters.AddWithValue("@id",id);
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
                            Bien = new Inmueble{
                                Id = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion))
                            },
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Arrendatario = new Inquilino{
                                Id = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Contrato> FiltrarContratos(Filtro filtro){
        List<Contrato> res = new List<Contrato>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT contratos.Id, Desde, Hasta, contratos.Estado, Mensualidad, InmuebleId, InquilinoId , inmuebles.Direccion , inquilinos.Nombre , inquilinos.Apellido
            FROM contratos 
            JOIN inmuebles ON inmuebles.Id = contratos.InmuebleId 
            JOIN inquilinos ON inquilinos.Id = contratos.InquilinoId
            WHERE (contratos.Desde <= @hasta)
            AND (contratos.Hasta >= @desde)
            AND contratos.Estado = 1";
            using( var command = new MySqlCommand( query , connection )) 
            {
                connection.Open();
                command.Parameters.AddWithValue("@desde",filtro.Desde);
                command.Parameters.AddWithValue("@hasta",filtro.Hasta);
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
                            Bien = new Inmueble{
                                Id = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion))
                            },
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            Arrendatario = new Inquilino{
                                Id = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(contrato);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Inmueble> InmueblesDisponiblesPorFecha(Filtro filtro){
        List<Inmueble> res = new List<Inmueble>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id, Direccion
            FROM inmuebles
            WHERE Id NOT IN (
                SELECT InmuebleId FROM contratos
                WHERE (Desde <= @hasta) AND (Hasta >= @desde) AND Estado = 1
            );";
            using( var command = new MySqlCommand( query , connection )) 
            {
                connection.Open();
                command.Parameters.AddWithValue("@desde",filtro.Desde);
                command.Parameters.AddWithValue("@hasta",filtro.Hasta);
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion))
                        };
                        res.Add(inmueble);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public bool ValidarFecha(Contrato contrato){
        bool res = false;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id FROM contratos WHERE (Desde <= @hasta) AND (Hasta >= @desde) AND Estado = 1 AND InmuebleId = @id";
            using( var command = new MySqlCommand( query , connection ))
            {
                connection.Open();
                command.Parameters.AddWithValue("@desde",contrato.Desde);
                command.Parameters.AddWithValue("@hasta",contrato.Hasta);
                command.Parameters.AddWithValue("@id",contrato.InmuebleId);
                using( var reader = command.ExecuteReader())
                {
                    if(!reader.Read())
                    {
                        res = true;
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public void Cancelar(Contrato contrato){
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"UPDATE contratos SET Estado = 0 WHERE Id = @id";
            using( var command = new MySqlCommand( query , connection ))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id",contrato.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}