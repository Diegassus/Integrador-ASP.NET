using System;
using System.Collections.Generic;
using Inmobiliaria.Models ;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Repositorios ;


public class RepositorioInquilino
{
    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";

    public RepositorioInquilino()
    {

    }

    public int EliminarInquilino(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM inquilinos WHERE Id = @id";
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

    public int EditarInquilino(Inquilino p)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"UPDATE inquilinos SET
            Nombre = @nombre,
            Apellido = @apellido,
            Dni = @dni,
            Telefono = @telefono,
            Correo = @correo,
            Estado = @estado 
            WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",p.Id);
                command.Parameters.AddWithValue("@nombre",p.Nombre);
                command.Parameters.AddWithValue("@apellido",p.Apellido);
                command.Parameters.AddWithValue("@dni",p.Dni);
                command.Parameters.AddWithValue("@telefono",p.Telefono);
                command.Parameters.AddWithValue("@correo",p.Correo);
                command.Parameters.AddWithValue("@estado",p.Estado);
                connection.Open();
                res = command.ExecuteNonQuery() ;
                connection.Close();
            }
        }
        return res ;
    }

    public int CrearInquilino(Inquilino i)
    {
        var res = -1 ;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO inquilinos (Apellido,Nombre,Dni,Telefono,Correo,Estado)
            VALUES (@apellido, @nombre, @dni, @telefono, @correo, @estado);
            SELECT LAST_INSERT_ID()"; // devuelve el id insertado
            using( var command = new MySqlCommand(query,connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@apellido",i.Apellido);
                command.Parameters.AddWithValue("@nombre" , i.Nombre);
                command.Parameters.AddWithValue("@dni",i.Dni);
                command.Parameters.AddWithValue("@telefono",i.Telefono);
                command.Parameters.AddWithValue("@correo",i.Correo);
                command.Parameters.AddWithValue("@estado",true);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res ;
    }

    public Inquilino ObtenerInquilino(int id)
    {
        Inquilino? res = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id,Nombre,Apellido,Dni,Telefono,Correo,Estado
            FROM inquilinos 
            WHERE Id = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        res = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Correo = reader.GetString(nameof(Inquilino.Correo)),
                            Estado = reader.GetBoolean(nameof(Inquilino.Estado))
                        };
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Inquilino> ObtenerInquilinos()
    {
        List<Inquilino> res = new List<Inquilino>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido FROM inquilinos";
            using( var command = new MySqlCommand( query , connection ))
            {
                connection.Open();
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Inquilino inquilino = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido))
                        };
                        res.Add(inquilino);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }
}