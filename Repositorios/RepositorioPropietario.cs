using MySql.Data.MySqlClient;
using Inmobiliaria.Models;
using System;
using System.Collections.Generic;

namespace Inmobiliaria.Repositorios;

public class RepositorioPropietario
{
    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";
    public RepositorioPropietario(){

    }

public int EliminarPropietario(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM propietarios WHERE Id = @id";
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

    public int EditarPropietario(Propietario p)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"UPDATE propietarios SET
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

    public int CrearPropietario(Propietario p)
    {
        var res = -1 ;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO propietarios (Apellido,Nombre,Dni,Telefono,Correo,Estado)
            VALUES (@apellido, @nombre, @dni, @telefono, @correo, @estado);
            SELECT LAST_INSERT_ID()"; // devuelve el id insertado
            using( var command = new MySqlCommand(query,connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@apellido",p.Apellido);
                command.Parameters.AddWithValue("@nombre" , p.Nombre);
                command.Parameters.AddWithValue("@dni",p.Dni);
                command.Parameters.AddWithValue("@telefono",p.Telefono);
                command.Parameters.AddWithValue("@correo",p.Correo);
                command.Parameters.AddWithValue("@estado",true);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res ;
    }

    public Propietario ObtenerPropietario(int id)
    {
        Propietario? res = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id,Nombre,Apellido,Dni,Telefono,Correo,Estado
            FROM propietarios
            WHERE Id = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        res = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Correo = reader.GetString(nameof(Propietario.Correo)),
                            Estado = reader.GetBoolean(nameof(Propietario.Estado))
                        };
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

    public List<Propietario> ObtenerPropietarios()
    {
        List<Propietario> res = new List<Propietario>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id, Nombre, Apellido FROM propietarios";
            using( var command = new MySqlCommand( query , connection ))
            {
                connection.Open();
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Propietario propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        };
                        res.Add(propietario);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }
}




