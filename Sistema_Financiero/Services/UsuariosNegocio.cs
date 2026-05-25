using System.Data;
using Sistema_Financiero.data;
using Sistema_Financiero.Models;

namespace Sistema_Financiero.Services
{
    public class UsuariosNegocio
    {
        private readonly UsuariosDatos _usuariosDatos;

        public UsuariosNegocio(UsuariosDatos usuariosDatos)
        {
            _usuariosDatos = usuariosDatos;
        }

        
        public DataTable MtdValidarUsuario(string usuario, string contrasena)
        {
            return _usuariosDatos.MtdValidarUsuario(usuario, contrasena);
        }

        
        public DataTable MtdConsultarUsuarios()
        {
            return _usuariosDatos.MtdConsultarUsuarios();
        }

        public DataTable MtdConsultarRoles()
        {
            return _usuariosDatos.MtdConsultarRoles();
        }

        public void MtdInsertarUsuario(UsuarioModelo u)
        {
            _usuariosDatos.MtdInsertarUsuario(u);
        }

        public void MtdActualizarUsuario(UsuarioModelo u)
        {
            _usuariosDatos.MtdActualizarUsuario(u);
        }

        public void MtdEliminarUsuario(int codigo)
        {
            _usuariosDatos.MtdEliminarUsuario(codigo);
        }
    }
}