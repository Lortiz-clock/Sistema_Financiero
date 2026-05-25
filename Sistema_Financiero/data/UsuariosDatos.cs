using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.data
{
    public class UsuariosDatos
    {
        private readonly ConexionDatos _conexionDatos;

        public UsuariosDatos(ConexionDatos conexionDatos)
        {
            _conexionDatos = conexionDatos;
        }

        
        public DataTable MtdValidarUsuario(string usuario, string contrasena)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ValidarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", usuario ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Clave", contrasena ?? string.Empty);
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al validar el usuario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado en el sistema: " + ex.Message);
            }
            return dt;
        }

        // ── MÉTODOS NUEVOS ──
        public DataTable MtdConsultarUsuarios()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarUsuarios", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al consultar usuarios: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar usuarios: " + ex.Message);
            }
            return dt;
        }

        public DataTable MtdConsultarRoles()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarRoles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al consultar roles: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al consultar roles: " + ex.Message);
            }
            return dt;
        }

        public void MtdInsertarUsuario(UsuarioModelo usuarioModelo)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_InsertarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", usuarioModelo.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoRol", usuarioModelo.CodigoRol);
                    cmd.Parameters.AddWithValue("@Nombre", usuarioModelo.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", usuarioModelo.Clave ?? "");
                    cmd.Parameters.AddWithValue("@Estado", usuarioModelo.Estado);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al insertar usuario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al insertar usuario: " + ex.Message);
            }
        }

        public void MtdActualizarUsuario(UsuarioModelo u)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoUsuario", u.CodigoUsuario);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", u.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoRol", u.CodigoRol);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", string.IsNullOrEmpty(u.Clave) ? (object)DBNull.Value : u.Clave);
                    cmd.Parameters.AddWithValue("@Estado", u.Estado);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al actualizar usuario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al actualizar usuario: " + ex.Message);
            }
        }

        public void MtdEliminarUsuario(int codigo)
        {
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_EliminarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoUsuario", codigo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al eliminar usuario: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al eliminar usuario: " + ex.Message);
            }
        }
    }
}