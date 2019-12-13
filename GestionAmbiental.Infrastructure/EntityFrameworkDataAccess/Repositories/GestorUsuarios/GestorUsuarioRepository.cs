using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.GestorUsuarios
{
    public class GestorUsuarioRepository : IGestorUsuarioReadOnlyRepository
    {
        private Context _context;

        public GestorUsuarioRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<InspeccionRol?> GetRolUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x=>x.usuario_red.Equals(usuario_red) && x.Estado.Equals("ACT"));
            if (usuario != null)
            {
                if (usuario.Id_Rol.Equals((int)InspeccionRol.AsesorMAP))
                    return InspeccionRol.AsesorMAP;
                else if (usuario.Id_Rol.Equals((int)InspeccionRol.AsesorSSO))
                    return InspeccionRol.AsesorSSO;
                else
                    return InspeccionRol.Inspector;
            }
            else
                return null;
        }

        public async Task<Usuario> GetUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => usuario_red.Equals(Util.GetDomain()+x.usuario_red) && x.Estado.Equals("ACT"));
            var output = new Usuario();
            output.Nombres = usuario.Nombres;
            output.Apellidos = usuario.Apellidos;
            output.Email = usuario.Email;
            output.UsuarioRed = usuario.usuario_red;
            output.IdUsuario = usuario.Id_Usuario;
            output.IdRol = usuario.Id_Rol;
            output.IdSuperior = usuario.Id_Superior;
            return output;
        }
        public async Task<int> GetIdUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => usuario_red.Equals(Util.GetDomain() + x.usuario_red) && x.Estado.Equals("ACT"));
            return usuario.Id_Usuario;
        }

    }
}
