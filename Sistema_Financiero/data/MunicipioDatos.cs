using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.data
{
    public class MunicipioDatos
    {
        private readonly ConexionDatos conexion;

        public MunicipioDatos(ConexionDatos conexionDatos)
        {
            conexion = conexionDatos;
        }

        public List<MunicipioModelo> MtdConsultarMunicipio()
        {
            var lista = new List<MunicipioModelo>();

            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarMunicipio", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new MunicipioModelo
                            {
                                CodigoMunicipio = Convert.ToInt32(dr["CodigoMunicipio"]),
                                CodigoDepartamento = dr["CodigoDepartamento"] != DBNull.Value ? Convert.ToInt32(dr["CodigoMunicipio"]) : 0,
                                Nombre = dr["Nombre"].ToString() ?? "",
                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error en datos al consultar municipios: " + ex.Message);
            }
            return lista;
        }

    }
}
