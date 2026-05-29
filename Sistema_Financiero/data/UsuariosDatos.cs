using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.data
{
    public class UsuariosDatos
    {
        // 1. Declaramos la variable global con el nombre exacto para que no marque NULL ni inexistente
        private readonly ConexionDatos _conexionDatos;

        public UsuariosDatos(ConexionDatos conexionDatos)
        {
            _conexionDatos = conexionDatos;
        }

        // ── VALIDAR USUARIO (Mantiene DataTable para tu AccountController) ──
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
            catch (Exception ex)
            {
                throw new Exception("Error al validar el usuario: " + ex.Message);
            }
            return dt;
        }

        // ── CONSULTAR USUARIOS (Listas con SqlDataReader como Sucursales) ──
        public List<UsuarioModelo> MtdConsultarUsuarios()
        {
            var lista = new List<UsuarioModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarUsuarios", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new UsuarioModelo
                            {
                                CodigoUsuario = Convert.ToInt32(dr["CodigoUsuario"]),
                                CodigoEmpleado = dr["CodigoEmpleado"] != DBNull.Value ? Convert.ToInt32(dr["CodigoEmpleado"]) : 0,
                                CodigoRol = dr["CodigoRol"] != DBNull.Value ? Convert.ToInt32(dr["CodigoRol"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                NombreUsuario = dr["Nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                NombreEmpleado = "Empleado #" + dr["CodigoEmpleado"],
                                NombreRol = "Rol #" + dr["CodigoRol"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en datos al consultar usuarios: " + ex.Message);
            }
            return lista;
        }

        // ── CONSULTAR ROLES ──
        public List<RolModelo> MtdConsultarRoles()
        {
            var lista = new List<RolModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarRoles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new RolModelo
                            {
                                CodigoRol = Convert.ToInt32(dr["CodigoRol"]),
                                Nombre = dr["Nombre"].ToString()!,
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en datos al consultar roles: " + ex.Message);
            }
            return lista;
        }

        // ── AGREGAR USUARIO (Con parámetros de salida OUTPUT) ──
        public bool MtdInsertarUsuario(UsuarioModelo usuario, out string MensajeSalida)
        {
            bool resultadofinal = false;
            MensajeSalida = "";
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_InsertarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", usuario.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoRol", usuario.CodigoRol);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    resultadofinal = pResultado.Value != DBNull.Value && Convert.ToBoolean(pResultado.Value);
                    MensajeSalida = pMensaje.Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                resultadofinal = false;
                MensajeSalida = "Ocurrió un error inesperado al agregar usuario: " + ex.Message;
            }
            return resultadofinal;
        }

        // ── EDITAR USUARIO (Con parámetros de salida OUTPUT) ──
        public string MtdEditarUsuario(UsuarioModelo usuario, out string MensajeSalida)
        {
            using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoUsuario", usuario.CodigoUsuario);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", usuario.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoRol", usuario.CodigoRol);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Clave", string.IsNullOrEmpty(usuario.Clave) ? (object)DBNull.Value : usuario.Clave);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(pResultado.Value);
                    string mensaje = pMensaje.Value?.ToString() ?? "Sin mensaje del servidor";

                    MensajeSalida = mensaje;
                    return mensaje;
                }
            }
        }

        // ── ELIMINAR USUARIO ──
        public string MtdEliminarUsuario(int codigo)
        {
            using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_EliminarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoUsuario", codigo);

                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(pResultado.Value);
                    string mensaje = pMensaje.Value?.ToString() ?? "Sin mensaje del servidor";

                    if (!resultado)
                        throw new Exception(mensaje);

                    return mensaje;
                }
            }
        }
    }
}