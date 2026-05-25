using Microsoft.Data.SqlClient;
using System.Data;

namespace Sistema_Financiero.data
{
    public class EmpleadosDatos
    {
        private readonly ConexionDatos _conexionDatos;

        public EmpleadosDatos(ConexionDatos conexionDatos)
        {
            _conexionDatos = conexionDatos;
        }

        public DataTable MtdConsultarEmpleados()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
            {
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return dt;
        }
    }
}
