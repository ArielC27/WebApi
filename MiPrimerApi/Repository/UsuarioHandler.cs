using MiPrimerApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace MiPrimerApi.Repository
{
    public static class UsuarioHandler
    {
        public const string ConnectionString = "Server=DESKTOP-NU2KG89;Database=SistemaGestion;Trusted_Connection=True";
        public static Usuario GetUser(string nombreUsuario)
        {
            Usuario usuario = new Usuario();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                var queryUsuario = "SELECT * FROM Usuario WHERE NombreUsuario = @nombreUsuario ";
                var nameUser = new SqlParameter("NombreUsuario", System.Data.SqlDbType.VarChar) { Value = nombreUsuario };
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(queryUsuario, sqlConnection))
                {
                    sqlCommand.Parameters.Add(nameUser);
                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                usuario.Id = Convert.ToInt32(dataReader["Id"]);
                                usuario.Nombre = dataReader["Nombre"].ToString();
                                usuario.Apellido = dataReader["Apellido"].ToString();
                                usuario.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                                usuario.Mail = dataReader["Mail"].ToString();

                            }
                        }
                        else
                        {
                            usuario.Id = 0;
                            usuario.Nombre = "";
                            usuario.Apellido = "";
                            usuario.NombreUsuario = "";
                            usuario.Contraseña = "";
                            usuario.Mail = "";
                        }
                    }
                }
                sqlConnection.Close();
            }
            return usuario;
        }
        public static bool CreateNewUser(Usuario usuario)
        {
            bool result = false;
            var ExisteUsuario = GetUser(usuario.NombreUsuario);
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                if (ExisteUsuario.Id == 0)
                {
                    string queryInsert = "INSERT INTO Usuario (Nombre, Apellido, NombreUsuario, Contraseña, Mail) " +
                        "VALUES (@nombre, @apellido, @nombreUsuario, @contraseña, @mail)";

                    SqlParameter nombre = new SqlParameter("Nombre", System.Data.SqlDbType.VarChar) { Value = usuario.Nombre };
                    SqlParameter apellido = new SqlParameter("Apellido", System.Data.SqlDbType.VarChar) { Value = usuario.Apellido };
                    SqlParameter nombreUsuario = new SqlParameter("NombreUsuario", System.Data.SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                    SqlParameter contraseña = new SqlParameter("Contraseña", System.Data.SqlDbType.VarChar) { Value = usuario.Contraseña };
                    SqlParameter mail = new SqlParameter("Mail", System.Data.SqlDbType.VarChar) { Value = usuario.Mail };

                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(nombre);
                        sqlCommand.Parameters.Add(apellido);
                        sqlCommand.Parameters.Add(nombreUsuario);
                        sqlCommand.Parameters.Add(contraseña);
                        sqlCommand.Parameters.Add(mail);

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
        public static bool UpdateUser(Usuario usuario)
        {
            bool result = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryUpdate = "UPDATE [SistemaGestion].[dbo].[Usuario]" +
                    "SET Nombre = @nombre, Apellido = @apellido, NombreUsuario = @nombreUsuario, Contraseña = @contraseña, Mail = @mail " +
                    "WHERE Id = @id ";

                SqlParameter nombre = new SqlParameter("nombre", System.Data.SqlDbType.VarChar) { Value = usuario.Nombre };
                SqlParameter apellido = new SqlParameter("apellido", System.Data.SqlDbType.VarChar) { Value = usuario.Apellido };
                SqlParameter nombreUsuario = new SqlParameter("nombreUsuario", System.Data.SqlDbType.VarChar) { Value = usuario.NombreUsuario };
                SqlParameter contraseña = new SqlParameter("contraseña", System.Data.SqlDbType.VarChar) { Value = usuario.Contraseña };
                SqlParameter mail = new SqlParameter("mail", System.Data.SqlDbType.VarChar) { Value = usuario.Mail };
                SqlParameter id = new SqlParameter("id", System.Data.SqlDbType.BigInt) { Value = usuario.Id };

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.Add(nombre);
                    sqlCommand.Parameters.Add(apellido);
                    sqlCommand.Parameters.Add(nombreUsuario);
                    sqlCommand.Parameters.Add(contraseña);
                    sqlCommand.Parameters.Add(mail);
                    sqlCommand.Parameters.Add(id);

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
        public static bool DeleteUser(int id)
        {
            bool result = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string queryDelete = "DELETE FROM Usuario WHERE Id = @id";
                SqlParameter sqlParameter = new SqlParameter("id", System.Data.SqlDbType.BigInt);
                sqlParameter.Value = id;

                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection))
                {
                    sqlCommand.Parameters.Add(sqlParameter);
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
