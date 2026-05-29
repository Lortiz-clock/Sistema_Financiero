using Sistema_Financiero.data;
using Sistema_Financiero.Models;
using System.Data;

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

        public List<UsuarioModelo> MtdConsultarUsuarios()
        {
            return _usuariosDatos.MtdConsultarUsuarios();
        }

        public List<RolModelo> MtdConsultarRoles()
        {
            return _usuariosDatos.MtdConsultarRoles();
        }

        public bool MtdInsertarUsuario(UsuarioModelo usuario, out string mensajeSalida)
        {
            return _usuariosDatos.MtdInsertarUsuario(usuario, out mensajeSalida);
        }

        public string MtdEditarUsuario(UsuarioModelo usuario, out string mensajeSalida)
        {
            return _usuariosDatos.MtdEditarUsuario(usuario, out mensajeSalida);
        }

        public string MtdEliminarUsuario(int codigo)
        {
            return _usuariosDatos.MtdEliminarUsuario(codigo);
        }
    }
}