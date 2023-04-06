using Inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Repositorios;

public class RepositorioPago{

    private string ConnectionString = "Server=localhost;User=root;Database=integradornet;SslMode=none";

    public RepositorioPago(){

    }

    public int EliminarPago(int id)
    {
        var res = 0 ;
        using ( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"DELETE FROM pagos WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res ;
    }

    public int EditarPago(Pago p)
    {
        var res = 0 ;
        using( MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"UPDATE pagos SET
            Nro = @nro,
            Fecha = @fecha,
            Importe = @importe
            WHERE Id = @id";
            using(var command  = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@nro",p.Nro);
                command.Parameters.AddWithValue("@fecha",p.Fecha);
                command.Parameters.AddWithValue("@importe",p.Importe);
                command.Parameters.AddWithValue("@id",p.Id);
                connection.Open();
                res = command.ExecuteNonQuery() ;
                connection.Close();
            }
        }
        return res == 1 ? res=p.ContratoId : -1;
    }

    public Pago ObtenerPago(int id){
        Pago res = null;
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT Id, Nro, Fecha, Importe, ContratoId
            FROM pagos
            WHERE Id = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        res = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Nro = reader.GetInt32(nameof(Pago.Nro)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId))
                        };
                    };
                }
            }
            connection.Close();
        }
            return res ;
    }
        
    

    public int CrearPago(Pago p){
        int res = -1;
        using(MySqlConnection connection = new MySqlConnection( ConnectionString))
        {
            var query = @"INSERT INTO pagos 
            (Nro, Fecha, Importe, ContratoId) 
            VALUES (@nro, @fecha, @importe, @contratoId)";
            using( var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@nro",p.Nro);
                command.Parameters.AddWithValue("@fecha",p.Fecha);
                command.Parameters.AddWithValue("@importe",p.Importe);
                command.Parameters.AddWithValue("@contratoId",p.ContratoId);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res == 1 ? p.ContratoId : -1 ;
    }

    public List<Pago> ObtenerPagos(int id){ // devuelve todos los pagos relacionados a un contrato
        List<Pago> res = new List<Pago>();
        using(MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var query = @"
            SELECT Id, Nro, Fecha, Importe , ContratoId
            FROM pagos
            WHERE ContratoId = @id";
            using(var command = new MySqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@id",id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Pago p = new Pago{
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Nro = reader.GetInt32(nameof(Pago.Nro)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId))
                        };
                        res.Add(p);
                    }
                }
                connection.Close();
            }
        }
        return res ;
    }

}