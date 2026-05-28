using Microsoft.AspNetCore.Components.Web;
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
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarClientes", conn))                     {
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
                using (SqlCommand cmd = new SqlCommand("usp_AgregarClientes", conn))
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

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    if (pResultado.Value != DBNull.Value && pResultado.Value != null)
                    {
                        resultadofinal = false;
                    }
                    MensajeSalidad = pMensaje.Value?.ToString() ?? "";
                    
                }
                
            }
            catch (Exception ex)
            {

                resultadofinal = false;
                MensajeSalidad = "Ocurrio un error inesperado al agregar: " + ex.Message;
            }
            return resultadofinal;
        }

        public string MtdActualizarCliente(ClientesModelo cliente, out string MensajeSalida)
        {
            using (SqlConnection conn = conexion.MtdConexionBDD())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarCliente", conn)) //falta el proceso 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@DPI", cliente.DPI);
                    cmd.Parameters.AddWithValue("@NIT", cliente.NIT);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    cmd.Parameters.AddWithValue("@Tipo", cliente.Tipo);

                    var pResultado = new SqlParameter("Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var pMensaje = new SqlParameter("Mensaje", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();

                    bool resultado = Convert.ToBoolean(pResultado.Value);
                    string mensaje = pMensaje.Value?.ToString() ?? "Sin mensajes del servidor";

                    MensajeSalida = mensaje;

                    return mensaje;
                }
                
            }
        }

        public string MtdEliminarCliente(int CodigoCliente)
        {
            using (SqlConnection conn = conexion.MtdConexionBDD())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_EliminarClientes", conn)) //agregar proseco
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("CodigoCliente", CodigoCliente);
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

        public List<ClientesModelo> MtdBuscarCliente(string nombre)
        {
            var lista = new List<ClientesModelo>();
            try
            {
                using (SqlConnection conn = conexion.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_BuscarClientes]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Nombre", nombre ?? "");
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ClientesModelo

                            {
                                CodigoCliente = dr["CodigoCliente"] != DBNull.Value ? Convert.ToInt32(dr["CodigoCliente"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                DPI = dr["DPI"].ToString(),
                                NIT = dr["NIT"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Direccion = dr["Direccion"].ToString(),



                            });
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {


                throw new Exception("Error en Datos al buscar empleado: " + ex.Message);
                
            }
            return lista;
        }
    
    }
}
