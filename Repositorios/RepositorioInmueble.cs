using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Repositorios;

public class RepositorioInmueble{
    
    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";
    public RepositorioInmueble(){

    }

    public int EliminarInmueble(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM inmuebles WHERE Id = @id";
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

    public int EditarInmueble(Inmueble i)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {//Lat, Lng, Tipo, Uso, Ambientes, Disponible, Direccion, Precio, PropietarioId
            var query = @"UPDATE inmuebles SET
            Lat = @lat,
            Lng = @lng,
            Tipo = @tipo,
            Uso = @uso,
            Ambientes = @ambientes,
            Disponible = @disponible,
            Direccion = @direccion,
            Precio = @precio,
            PropietarioId = @duenio
            WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@lat",i.Lat);
                command.Parameters.AddWithValue("@lng" , i.Lng);
                command.Parameters.AddWithValue("@tipo",i.Tipo);
                command.Parameters.AddWithValue("@uso",i.Uso);
                command.Parameters.AddWithValue("@ambientes",i.Ambientes);
                command.Parameters.AddWithValue("@disponible",i.Disponible);
                command.Parameters.AddWithValue("@direccion",i.Direccion);
                command.Parameters.AddWithValue("@precio",i.Precio);
                command.Parameters.AddWithValue("@duenio",i.PropietarioId);
                command.Parameters.AddWithValue("@id",i.Id);
                connection.Open();
                res = command.ExecuteNonQuery() ;
                connection.Close();
            }
        }
        return res ;
    }

    public int CrearInmueble(Inmueble i)
    {
        var res = -1 ;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO inmuebles 
            (Lat, Lng, Tipo, Uso, Ambientes, Disponible, Direccion, Precio, PropietarioId )
            VALUES (@lat, @lng, @tipo, @uso, @ambientes, @disponible, @direccion, @precio, @propietarioId);
            SELECT LAST_INSERT_ID()"; // devuelve el id insertado
            using( var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@lat",i.Lat);
                command.Parameters.AddWithValue("@lng" , i.Lng);
                command.Parameters.AddWithValue("@tipo",i.Tipo);
                command.Parameters.AddWithValue("@uso",i.Uso);
                command.Parameters.AddWithValue("@ambientes",i.Ambientes);
                command.Parameters.AddWithValue("@disponible",true);
                command.Parameters.AddWithValue("@direccion",i.Direccion);
                command.Parameters.AddWithValue("@precio",i.Precio);
                command.Parameters.AddWithValue("@propietarioId",i.PropietarioId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res ;
    }

    public Inmueble ObtenerInmueble(int id)
    {
        Inmueble? res = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT i.Id, Lat, Lng, Uso, Tipo, Ambientes, Disponible, Direccion, Precio, PropietarioId, 
            p.Apellido, p.Nombre, p.Dni, p.Telefono, p.Correo, p.Estado
            FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.Id
            WHERE i.Id = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        res = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Lat = reader.GetString(nameof(Inmueble.Lat)),
                            Lng = reader.GetString(nameof(Inmueble.Lng)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Duenio = new Propietario{
                                Id = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Correo = reader.GetString(nameof(Propietario.Correo)),
                            }
                            
                        };
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Inmueble> InmueblesPropietario(int id)
    {
        List<Inmueble> res = new List<Inmueble>();
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT i.Id, Tipo, Uso, Direccion,Disponible, Precio, PropietarioId ,p.Nombre, p.Apellido FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id WHERE PropietarioId = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Duenio = new Propietario{
                                Id = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(inmueble);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Inmueble> InmueblesDisponibles()
    {
        List<Inmueble> res = new List<Inmueble>();
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT i.Id, Tipo, Uso, Direccion,Disponible, Precio, PropietarioId ,p.Nombre, p.Apellido FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id WHERE Disponible = @bool";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@bool",true);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Duenio = new Propietario{
                                Id = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(inmueble);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Inmueble> ObtenerInmuebles()
    {
        List<Inmueble> res = new List<Inmueble>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT i.Id, Tipo, Uso, Direccion, Precio, Disponible,PropietarioId, p.Nombre, p.Apellido
            FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id";
            using( var command = new MySqlCommand( query , connection )) 
            {
                connection.Open();
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Tipo = reader.GetInt32(nameof(Inmueble.Tipo)),
                            Uso = reader.GetInt32(nameof(Inmueble.Uso)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Duenio = new Propietario{
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        };
                        res.Add(inmueble);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }


}