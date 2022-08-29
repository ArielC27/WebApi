using MiPrimerApi.Model;
using System.Data.SqlClient;

namespace MiPrimerApi.Repository
{
    public static class VentaHandler
    {
        public const string ConnectionString = "Server=DESKTOP-NU2KG89;Database=SistemaGestion;Trusted_Connection=True";
        public static List<Venta> GetVentas()
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                var queryVenta = "SELECT * FROM Venta";
                using (SqlCommand sqlCommand = new SqlCommand(queryVenta, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Venta venta = new Venta();
                                venta.Id = Convert.ToInt32(dataReader["Id"]);
                                venta.Comentarios = dataReader["Comentarios"].ToString();

                                ventas.Add(venta);
                            }
                        }
                    }
                }
                sqlConnection.Close();
            }
            return ventas;
        }
        public static bool CreateNewSale(List<Producto> producto, Venta venta)
        {
            bool result = false;
            int idVenta = 0;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string queryInsert = "INSERT INTO Venta (Comentarios, IdUsuario) VALUES (@comentarios, @IdUsuario)";

                SqlParameter comentarios = new SqlParameter("Comentarios", System.Data.SqlDbType.VarChar) { Value = venta.Comentarios };
                SqlParameter IdUsuario = new SqlParameter("IdUsuario", System.Data.SqlDbType.VarChar) { Value = venta.IdUsuario };
                SqlParameter[] ParametrosVentas = new SqlParameter[] { comentarios, IdUsuario };
                using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                {
                    sqlCommand.Parameters.AddRange(ParametrosVentas);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        result = true;
                    }
                }

                //Selecciono el último Id
                string query = "SELECT TOP (1) [Id] FROM [Venta] ORDER BY Id Desc";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                idVenta = Convert.ToInt32(dataReader["ID"]);
                            }
                        }
                    }
                }

                foreach (var prod in producto)
                {
                    // itero la lista de productos y cargo productos vendidos
                    string queryInsertProductoVendido = "INSERT INTO ProductoVendido (Stock, IdProducto, IdVenta) " +
                                                         "VALUES (@Stock, @IdProducto, @IdVenta)";
                    SqlParameter Stock = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = prod.Stock };
                    SqlParameter IdProducto = new SqlParameter("IdProducto", System.Data.SqlDbType.Int) { Value = prod.Id };
                    SqlParameter IdVenta = new SqlParameter("IdVenta", System.Data.SqlDbType.Int) { Value = idVenta };

                    SqlParameter[] ParametrosProductosVendidos = new SqlParameter[] { Stock, IdProducto, IdVenta };
                    
                    using (SqlCommand sqlCommand = new SqlCommand(queryInsertProductoVendido, sqlConnection))
                    {
                        sqlCommand.Parameters.AddRange(ParametrosProductosVendidos);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            result = true;
                        }
                    }

                    //Comienza el update de la Tabla Producto
                    string QueryProductos = "UPDATE PRODUCTO SET Stock = (Stock - @Stock) WHERE ID = @ID";

                    SqlParameter StockaDescontar = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = prod.Stock };
                    SqlParameter IdProductoCorresponde = new SqlParameter("ID", System.Data.SqlDbType.Int) { Value = prod.Id };
                    SqlParameter[] ParametersProductoS = new SqlParameter[] { StockaDescontar, IdProductoCorresponde };

                    using (SqlCommand sqlCommand = new SqlCommand(QueryProductos, sqlConnection))
                    {
                        sqlCommand.Parameters.AddRange(ParametersProductoS);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            result = true;
                        }
                    }
                }
                sqlConnection.Close();
            }

            return result;
        }
        //public static bool DeleteSale(int idVenta)
        //{
        //    bool result = false;
        //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        //    {
        //        string queryDelete = "DELETE FROM Venta WHERE Id = @id";
        //        SqlParameter sqlParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt);
        //        sqlParameter.Value = idVenta;

        //        sqlConnection.Open();
        //        using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
        //        {
        //            sqlCommand.Parameters.Add(sqlParameter);
        //            int numberOfRows = sqlCommand.ExecuteNonQuery();
        //            if (numberOfRows > 0)
        //            {
        //                result = true;
        //            }
        //        }
        //        sqlConnection.Close();
        //    }
        //    return result;
        //}
    }

}
