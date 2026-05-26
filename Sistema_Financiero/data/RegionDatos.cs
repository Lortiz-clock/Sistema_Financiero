using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.data
{
    public class RegionDatos
    {
        private readonly ConexionDatos _conexionDatos;

        public RegionDatos(ConexionDatos conexionDatos)
        {
            _conexionDatos = conexionDatos;
        }

        public bool MtdAgregarRegion(RegionModelo region, out string MensajeSalida)
        {
            bool resultadofinal = false;
            MensajeSalida = "";

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_AgregarRegion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", region.Nombre);

                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

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
    }
}