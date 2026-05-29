using Microsoft.Data.SqlClient;
using Sistema_Financiero.Models;
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

        public bool MtdAgregarEmpleado(EmpleadosModelo empleado, out string MensajeSalida)
        {
            bool resultadofinal = false;
            MensajeSalida = "";

            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_AgregarEmpleados", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoSucursal", empleado.CodigoSucursal);
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono", empleado.Telefono);
                    cmd.Parameters.AddWithValue("@FechaEntrada", empleado.FechaEntrada);
                    cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                    cmd.Parameters.AddWithValue("@Estado", empleado.Estado);
                                       
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

        public List<EmpleadosModelo> MtdConsultarEmpleados()
        {
            var lista = new List<EmpleadosModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ConsultarEmpleados", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new EmpleadosModelo
                            {
                                CodigoEmpleado = Convert.ToInt32(dr["CodigoEmpleado"]),
                                CodigoSucursal = dr["CodigoSucursal"] != DBNull.Value ? Convert.ToInt32(dr["CodigoSucursal"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                FechaEntrada = Convert.ToDateTime(dr["FechaEntrada"]),
                                Direccion = dr["Direccion"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                                Cargo = dr["Cargo"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Datos al consultar empleados: " + ex.Message);
            }
            return lista;
        }

        public List<EmpleadosModelo> MtdBuscarEmpleado(string nombre)
        {
            var lista = new List<EmpleadosModelo>();
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_BuscarEmpleados", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombre ?? "");

                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new EmpleadosModelo
                            {
                                CodigoEmpleado = Convert.ToInt32(dr["CodigoEmpleado"]),
                                CodigoSucursal = dr["CodigoSucursal"] != DBNull.Value ? Convert.ToInt32(dr["CodigoSucursal"]) : 0,
                                Nombre = dr["Nombre"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                FechaEntrada = Convert.ToDateTime(dr["FechaEntrada"]),
                                Direccion = dr["Direccion"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                                Cargo = dr["Cargo"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
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

        public bool MtdActualizarEmpleado(EmpleadosModelo empleados, out string mensajeSalida)
        {
            bool resultadofinal = false;
            mensajeSalida = "";
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", empleados.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoSucursal", empleados.CodigoSucursal);
                    cmd.Parameters.AddWithValue("@Nombre", empleados.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono", empleados.Telefono);
                    cmd.Parameters.AddWithValue("@FechaEntrada", empleados.FechaEntrada);
                    cmd.Parameters.AddWithValue("@Direccion", empleados.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleados.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Cargo", empleados.Cargo);
                    cmd.Parameters.AddWithValue("@Estado", empleados.Estado);

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
                    mensajeSalida = pMensaje.Value?.ToString() ?? "";
                }

            }
            catch (Exception ex)
            {

                resultadofinal = false;
                mensajeSalida = "Ocurrio un error inesperado: " + ex.Message;
            }
            return resultadofinal;
        }

        public bool MtdEliminarEmpleado(int Codigo, out string mensajeSalida)
        {
            bool resultadoFinal = false;
            mensajeSalida = "";
            try
            {
                using (SqlConnection conn = _conexionDatos.MtdConexionBDD())
                using (SqlCommand cmd = new SqlCommand("usp_EliminarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", Codigo);

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


