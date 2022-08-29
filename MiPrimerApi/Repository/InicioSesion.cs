using MiPrimerApi.Model;
using System.Data.SqlClient;
using System.Data;

namespace MiPrimerApi.Repository
{
    public static class InicioSesion
    {
        public const string ConnectionString = "Server=DESKTOP-NU2KG89;Database=SistemaGestion;Trusted_Connection=True";
        public static Usuario IniciarSesion(string nombreUsuario, string contraseña)
        {
            Usuario usuario = new Usuario();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                var query ="SELECT * FROM Usuario WHERE NombreUsuario = @nombreUsuario AND Contraseña = @contraseña";
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("NombreUsuario", SqlDbType.VarChar) { Value = nombreUsuario });
                    sqlCommand.Parameters.Add(new SqlParameter("Contraseña", SqlDbType.VarChar) { Value = contraseña });
                    sqlCommand.ExecuteNonQuery();
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
                                usuario.Mail = dataReader["Mail"].ToString();
                                usuario.Contraseña = dataReader["Contraseña"].ToString();
                            }
                        }
                        else
                        {
                            usuario.Id = 0;
                            usuario.Nombre = "Usuario o contraseña incorrecta";
                        }
                    }
                }
                sqlConnection.Close();
            }
            return usuario;
        }
    }
}
