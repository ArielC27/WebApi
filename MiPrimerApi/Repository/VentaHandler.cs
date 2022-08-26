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
        public static bool CreateNewSale(/*List<Producto> productos, int idUsuario,*/ Venta venta)
        {
            bool result = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryVenta = "INSERT INTO Venta (Comentarios) VALUES (@comentarios)";
                //string queryPV = "SELECT * FROM Producto AS p INNER JOIN ProductoVendido AS pv ON @pv.IdProducto = @p.Id ";

                SqlParameter comentarios = new SqlParameter("Comentarios", System.Data.SqlDbType.VarChar) { Value = venta.Comentarios };

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryVenta, sqlConnection))
                {
                    sqlCommand.Parameters.Add(comentarios);

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
