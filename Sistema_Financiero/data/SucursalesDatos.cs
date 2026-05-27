using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;
namespace Sistema_Financiero.data
{
    public class SucursalesDatos
    {
        private readonly ConexionDatos conexion;

        public SucursalesDatos(ConexionDatos conexionDatos)
        {
            conexion = conexionDatos;
        }

        public List<SucursalesModelo> MtdConsultarSucursal()
        {
            var lista = new List<SucursalesModelo>();

            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarSucursales", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                       while (dr.Read())
                    {
                            lista.Add(new SucursalesModelo
                            {
                                CodigoSucursal = Convert.ToInt32(dr["CodigoSucursal"]),
                                CodigoMunicipio = dr["CodigoMunicipio"] != DBNull.Value ? Convert.ToInt32(dr["CodigoMunicipio"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                    }
                }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error en datos al consultar sucursales: " + ex.Message);
            }
            return lista;
        }
       

        public bool MtdAgregarSucursal(SucursalesModelo sucursal, out string MensajeSalida)
        {
            bool resultadofinal = false;
            MensajeSalida = "";

            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_AgregarSucursal", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoMunicipio", sucursal.CodigoMunicipio);
                    cmd.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                    cmd.Parameters.AddWithValue("@Estado", sucursal.Estado ?? false);

                    SqlParameter pResultado = new SqlParameter("@Resultado", System.Data.SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    if (pResultado.Value != DBNull.Value && pResultado.Value != null)
                    {
                        resultadofinal = Convert.ToBoolean(pResultado.Value);
                    }
                    else
                    {
                        resultadofinal = false;
                    }

                    MensajeSalida = pMensaje.Value?.ToString() ?? "";
                    MensajeSalida = pMensaje.Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {

                resultadofinal = false;
                MensajeSalida = "Ocurrio un error inesperado al agregar: " + ex.Message;
            }
            return resultadofinal;
        }
       
        public string MtdEditarSucursal(SucursalesModelo sucursal, out string MensajeSalida)
        {
            using (SqlConnection conn = conexion.MtdConexionBDD())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_EditarSucursal", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoSucursal", sucursal.CodigoSucursal);
                    cmd.Parameters.AddWithValue("@CodigoMunicipio", sucursal.CodigoMunicipio);
                    cmd.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                    cmd.Parameters.AddWithValue("@Estado", sucursal.Estado);

                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

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

        public string MtdEliminarSucursal(int CodigoSucursal)
        {            
            using (SqlConnection conn = conexion.MtdConexionBDD())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand ("usp_EliminarSucursal", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("CodigoSucursal", CodigoSucursal);
                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(pResultado.Value);
                    string mensaje = pMensaje.Value.ToString() ?? "Sin mensaje del servidor";

                    if (!resultado)
                        throw new Exception(mensaje);
                    return mensaje;
 
                        
                }
            }
        }

        public List<SucursalesModelo> MtdBuscarSucursal(string nombre)
        {
            var lista = new List<SucursalesModelo>();
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_BuscarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombre ?? "");

                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new SucursalesModelo
                            {                                
                                CodigoSucursal = dr["CodigoSucursal"] != DBNull.Value ? Convert.ToInt32(dr["CodigoSucursal"]) : 0,
                                CodigoMunicipio = dr["CodigoMunicipio"] != DBNull.Value ? Convert.ToInt32(dr["CodigoSucursal"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Datos al buscar sucursal: " + ex.Message);
            }
            return lista;
        }

    }
}