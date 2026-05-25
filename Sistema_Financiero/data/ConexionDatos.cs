using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Sistema_Financiero.data
{
    public class ConexionDatos
    {
        private readonly string CadenaConexion;

      
        public ConexionDatos(IConfiguration configuration)
        {
            CadenaConexion = configuration
                .GetConnectionString("CadenaConexionBDD") ??string.Empty;
        }

        public SqlConnection MtdConexionBDD()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}
