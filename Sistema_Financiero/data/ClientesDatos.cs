using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
using System.Data;

namespace Sistema_Financiero.data
{
    public class ClientesDatos
    {
        private readonly ConexionDatos conexion;
        public ClientesDatos(ConexionDatos conexionDatos)
        {
            conexion = conexionDatos;
        }

        public List<ClientesModelo> MtdConsultarCliente()
        {
            var lista = new List<ClientesModelo>();
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                {
                    using (SqlCommand cmd =SqlCommand("", conn))        //pendiente nombre proceso almacenado
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new ClientesModelo
                                {
                                    CodigoCliente = Convert.ToInt32(dr["CodigoCliente"]),
                                    CodigoMunicipio = dr["CodigoMunicipio"] != DBNull.Value ? Convert.ToInt32(dr["CodigoMunicipio"]) : 0,
                                    Nombre = dr["Nombre"].ToString(),
                                    DPI = dr["DPI"].ToString(),
                                    NIT = dr["NIT"].ToString(),
                                    Correo= dr["Correo"].ToString(),
                                    Direccion = dr["Direccion"].ToString(),
                                    Tipo = dr["Tipo"].ToString()
                                });
                            }
                        }
                    }
                 }
            }
            catch (Exception ex)
            {

                throw new Exception("Erroren datos al consultar Clientes: " + ex.Message);
            }
            return lista;
        }
    
        public bool MtdAgregarCliente(ClientesModelo cliente, out string MensajeSalidad)
        {
            bool resultadofinal = false;
            MensajeSalidad = "";
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))//falta agregar nombre de proceso
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoMunicipio", cliente.CodigoMunicipio);
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@DPI", cliente.DPI);
                        cmd.Parameters.AddWithValue("@NIT", cliente.NIT);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                        cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                        cmd.Parameters.AddWithValue("@Tipo", cliente.Tipo);

                        SqlParameter pResultado = new SqlParameter("@Resultado", System.Data.SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                          
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    
    }
}
