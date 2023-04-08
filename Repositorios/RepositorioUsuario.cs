using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Repositorios;

public class RepositorioUsuario {
    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";

    public int EliminarUsuario(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM usuarios WHERE Id = @id";
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

    public int EditarUsuario(Usuario p)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"UPDATE usuarios SET
            Nombre = @nombre,
            Apellido = @apellido,
            Dni = @dni,
            Telefono = @telefono,
            Correo = @correo,
            Estado = @estado 
            Clave = @clave,
            Avatar = @avatar,
            Rol = @rol
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
                command.Parameters.AddWithValue("@clave",p.Clave);
                command.Parameters.AddWithValue("@avatar",p.Avatar);
                command.Parameters.AddWithValue("@rol",p.Rol);
                connection.Open();
                res = command.ExecuteNonQuery() ;
                connection.Close();
            }
        }
        return res ;
    }

    public int CrearUsuario(Usuario u)
    {
        var res = -1 ;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO usuarios (Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol)
            VALUES (@apellido, @nombre, @dni, @telefono, @correo, @estado,@clave,@avatar,@rol);
            SELECT LAST_INSERT_ID()"; // devuelve el id insertado
            using( var command = new MySqlCommand(query,connection))
            {
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@apellido",u.Apellido);
                command.Parameters.AddWithValue("@nombre" , u.Nombre);
                command.Parameters.AddWithValue("@dni",u.Dni);
                command.Parameters.AddWithValue("@telefono",u.Telefono);
                command.Parameters.AddWithValue("@correo",u.Correo);
                command.Parameters.AddWithValue("@estado",true);
                command.Parameters.AddWithValue("@clave",u.Clave);
                command.Parameters.AddWithValue("@avatar",u.Avatar);
                command.Parameters.AddWithValue("@rol",u.Rol);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res ;
    }

    public List<Usuario> ObtenerUsuarios()
    {
        List<Usuario> res = new List<Usuario>();
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"SELECT Id, Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol FROM usuarios";
            using( var command = new MySqlCommand( query , connection ))
            {
                connection.Open();
                using( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Usuario user = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Dni = reader.GetString(nameof(Usuario.Dni)),
                            Telefono = reader.GetString(nameof(Usuario.Telefono)),
                            Correo = reader.GetString(nameof(Usuario.Correo)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol))
                        };
                        res.Add(user);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }    

    public Usuario ObtenerPorId(int id){
        Usuario? u = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString)){
            string sql = @"SELECT Id, Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol FROM usuarios WHERE Id = @id";
            using(MySqlCommand command = new MySqlCommand(sql,connection)){
                command.Parameters.AddWithValue("@id",id);
                command.CommandType = System.Data.CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    u = new Usuario
                    {
                        Id = reader.GetInt32(nameof(Usuario.Id)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Telefono = reader.GetString(nameof(Usuario.Telefono)),
                        Correo = reader.GetString(nameof(Usuario.Correo)),
                        Estado = reader.GetBoolean(nameof(Usuario.Estado)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol))    
                    };
                }
                connection.Close();
            }
        }
        return u ;
    }   
    public Usuario ObtenerPorCorreo(string correo){
        Usuario? u = null ;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString)){
            string sql = @"SELECT Id, Apellido,Nombre,Dni,Telefono,Correo,Estado,Clave,Avatar,Rol FROM usuarios WHERE correo = @correo";
            using(MySqlCommand command = new MySqlCommand(sql,connection)){
                command.Parameters.AddWithValue("@correo",correo);
                command.CommandType = System.Data.CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if(reader.Read())
                {
                    u = new Usuario
                    {
                        Id = reader.GetInt32(nameof(Usuario.Id)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Telefono = reader.GetString(nameof(Usuario.Telefono)),
                        Correo = reader.GetString(nameof(Usuario.Correo)),
                        Estado = reader.GetBoolean(nameof(Usuario.Estado)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol))    
                    };
                }
                connection.Close();
            }
        }
        return u ;
    }   

    
}