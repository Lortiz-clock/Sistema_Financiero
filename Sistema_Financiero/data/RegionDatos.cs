using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sistema_Financiero.data
{
    public class RegionDatos
    {
        private readonly ConexionDatos _conexionDatos;

        public RegionDatos(ConexionDatos conexionDatos)
        {
            _conexionDatos = conexionDatos;
        }
        
       public string MtdAgregarRegion(RegionModelo region)
        {
            using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("usp_AgregarRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", region.Nombre);

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
                        throw new ApplicationException(mensaje);

                    return mensaje;
                }
            }
        }
        public List<RegionModelo> MtdConsultarRegion()
        {
            var lista = new List<RegionModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarRegion", conn)) // 📝 Verifica si en tu BDD tiene la "t" o es "usp_ConsularRegion"
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new RegionModelo
                            {
                                CodigoRegion = Convert.ToInt32(dr["CodigoRegion"]),
                                Nombre = dr["Nombre"].ToString()!
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Datos al consultar regiones: " + ex.Message);
            }
            return lista;
        }

        public List<RegionModelo> MtdBuscarRegion(string nombre)
        {
            var lista = new List<RegionModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_BuscarRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombre ?? "");

                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new RegionModelo
                            {
                                CodigoRegion = Convert.ToInt32(dr["CodigoRegion"]),
                                Nombre = dr["Nombre"].ToString()!
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Datos al buscar región: " + ex.Message);
            }
            return lista;
        }

        public bool MtdEliminarRegion(int Codigo, out string mensajeSalida)
        {
            bool resultadoFinal = false;
            mensajeSalida = "";
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_EliminarRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoRegion", Codigo);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

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

        public string MtdEditarRegion(RegionModelo region)
        {
            using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("usp_ActualizarRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoRegion", region.CodigoRegion);
                    cmd.Parameters.AddWithValue("Nombre", region.Nombre);

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

                    if (!resultado)
                        throw new ApplicationException(mensaje);
                    return mensaje;
                }
            }
        }

    }
}