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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoMunicipio", sucursal.CodigoMunicipio);
                    cmd.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                    cmd.Parameters.AddWithValue("@Estado", sucursal.Estado ?? false);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
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


                    resultadofinal = pResultado.Value != null && Convert.ToBoolean(pResultado.Value);
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
       
        public bool MtdActualizarSucursal(SucursalesModelo sucursal, out string MensajeSalida)
        {          
            
                bool resultadofinal = false;
                MensajeSalida = "";
              try
              {
                    using (SqlConnection conn = conexion.MtdConexionBDD())
                    using (SqlCommand cmd = new SqlCommand("usp_ActualizarSucursal", conn))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoSucursal", sucursal.CodigoSucursal);
                    cmd.Parameters.AddWithValue("@CodigoMunicipio", sucursal.CodigoMunicipio);
                    cmd.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                    cmd.Parameters.AddWithValue("@Estado", sucursal.Estado);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
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

                    resultadofinal = pResultado.Value != null && Convert.ToBoolean(pResultado.Value);
                    MensajeSalida = pMensaje.Value?.ToString() ?? "";
                }

            }
            catch (Exception ex)
            {

                resultadofinal = false;
                MensajeSalida = "Ocurrio un error inesperado: " + ex.Message;
            }
            return resultadofinal;
        }
    
     

        public bool MtdEliminarSucursal(int CodigoSucursal, out string mensajeSalida)
        {
            bool resultadoFinal = false;
            mensajeSalida = "";
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand ("usp_EliminarSucursal", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("CodigoSucursal", CodigoSucursal);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    resultadoFinal = pResultado.Value != null && Convert.ToBoolean(pResultado.Value);
                    mensajeSalida = pMensaje.Value?.ToString() ?? "";
                                           
                }
            }
            catch (Exception ex)
            {
                resultadoFinal = false;
                mensajeSalida = "Ocurrio un error inesperado al eliminar: " + ex.Message;
            }
            return resultadoFinal;
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