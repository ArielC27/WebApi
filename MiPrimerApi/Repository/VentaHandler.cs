using MiPrimerApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimerApi.Repository
{
    public static class VentaHandler
    {
        public const string ConnectionString = "Server=DESKTOP-NU2KG89;Database=SistemaGestion;Trusted_Connection=True";
        public static List<Venta> GetVentas(int IdUsuario)
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                var queryVenta = "SELECT V.Id, V.Comentarios FROM Venta AS V " +
                "INNER JOIN ProductoVendido AS PV ON PV.IdVenta = V.Id " +
                "INNER JOIN Producto AS P ON PV.IdProducto = P.Id " +
                "WHERE P.IdUsuario = @IdUsuario";
                var sqlParamenter = new SqlParameter("IdUsuario", SqlDbType.BigInt) { Value = IdUsuario };

                using (SqlCommand sqlCommand = new SqlCommand(queryVenta, sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.Add(sqlParamenter);
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
                SqlParameter IdUsuario = new SqlParameter("IdUsuario", System.Data.SqlDbType.Int) { Value = venta.IdUsuario };
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
        public static bool EliminarVenta(Venta venta)
        {
            bool result = false;
            //Busco todos los Productos vendidos relacionados con el IdVentas y armo una lista
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                List<ProductoVendido> ListaProductosVendidos = new List<ProductoVendido>();
                string querydeleteProductoVendido = "SELECT * FROM ProductoVendido WHERE IdVenta = @IdVenta";
                var sqlParamenter = new SqlParameter("IdVenta", SqlDbType.BigInt) { Value = venta.Id };

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(querydeleteProductoVendido, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParamenter);   
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();
                                productoVendido.Id = Convert.ToInt32(dataReader["Id"]);
                                productoVendido.Stock = Convert.ToInt32(dataReader["Stock"]);
                                productoVendido.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                productoVendido.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                ListaProductosVendidos.Add(productoVendido);
                            }
                        }
                    }
                }
                //Itero la lista obtenida de Productos vendidos para restablecer el stock de la tabla Producto
                foreach (var productoVendido in ListaProductosVendidos)
                {
                    //Actualizo tabla Productos
                    string QueryProductos = "UPDATE PRODUCTO SET Stock = (Stock + @Stock) WHERE ID = @ID";
                    SqlParameter StockaSumar = new SqlParameter("Stock", System.Data.SqlDbType.Int) { Value = productoVendido.Stock };
                    SqlParameter IdProductoCorresponde = new SqlParameter("ID", System.Data.SqlDbType.Int) { Value = productoVendido.IdProducto };
                    SqlParameter[] ParametrosProductosVendidos = new SqlParameter[] { StockaSumar, IdProductoCorresponde };

                    using (SqlCommand sqlCommand = new SqlCommand(QueryProductos, sqlConnection))
                    {
                        sqlCommand.Parameters.AddRange(ParametrosProductosVendidos);
                        int numberOfRows = sqlCommand.ExecuteNonQuery();
                        if (numberOfRows > 0)
                        {
                            result = true;
                        }
                    }
                }
                //Elimino Producto Vendido
                string queryDeletePv = "DELETE FROM ProductoVendido WHERE IdVenta = @idVenta";
                var sqlParamenterPv = new SqlParameter("idVenta", SqlDbType.BigInt) { Value = venta.Id };
                using (SqlCommand sqlCommand = new SqlCommand (queryDeletePv, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParamenterPv);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        result = true;
                    }
                }
                //Elimino Venta
                string queryDeleteVenta = "DELETE FROM Venta WHERE Id = @id";
                var sqlParamenterV = new SqlParameter("id", SqlDbType.BigInt) { Value = venta.Id };
                using (SqlCommand sqlCommand = new SqlCommand(queryDeleteVenta, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParamenterV);
                    int numberOfRows = sqlCommand.ExecuteNonQuery();
                    if (numberOfRows > 0)
                    {
                        result = true;
                    }
                }
                sqlConnection.Close();
            }
            return result;
        }

    }

}
