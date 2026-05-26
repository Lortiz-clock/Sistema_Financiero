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

        public bool MtdAgregarSucursal(SucursalesModelo sucursal, out string MensajeSalida)
        {
            bool resultadofinal = false;
            MensajeSalida = "";

            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                    using (SqlCommand cmd = new SqlCommand("usp_AgregarSucursal"))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoMunicipio", sucursal.CodigoMunicipio);
                    cmd.Parameters.AddWithValue("@Nombre", sucursal.Nombre);
                    cmd.Parameters.AddWithValue("@Direccion", sucursal.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", sucursal.Telefono);
                    cmd.Parameters.AddWithValue("@Estado", sucursal.Estado);

                    SqlParameter pResultado = new SqlParameter("@Resultado", System.Data.SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar)
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

       /* public List<SucursalesModelo> MtdConsultarSucursal()
        {
            var lista = new List<SucursalesModelo>();
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using(SqlCommand cmd =new SqlCommand("usp_ConsultarSucursales"))
            }
            catch (Exception)
            {

                throw;
            }

        }*/
    }
}
