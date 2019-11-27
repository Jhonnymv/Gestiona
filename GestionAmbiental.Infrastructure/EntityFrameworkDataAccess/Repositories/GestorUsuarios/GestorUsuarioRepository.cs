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

        public async Task<InspeccionRol> GetRolUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x=>x.usuario_red.Equals(usuario_red));
            if (usuario.Id_Rol.Equals((int)InspeccionRol.VerificadorMAP))
                return InspeccionRol.VerificadorMAP;
            else if (usuario.Id_Rol.Equals((int)InspeccionRol.VerificadorSSO))
                return InspeccionRol.VerificadorSSO;
            else
                return InspeccionRol.Inspector;
        }

        public async Task<Usuario> GetUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => usuario_red.Equals(Util.GetDomain()+x.usuario_red));
            var output = new Usuario();
            output.Nombres = usuario.Nombres;
            output.Apellidos = usuario.Apellidos;
            output.Email = usuario.Email;
            output.usuario_red = usuario.usuario_red;
            output.Id_Usuario = usuario.Id_Usuario;
            output.Id_Rol = usuario.Id_Rol;
            output.Id_Superior = usuario.Id_Superior;
            return output;
        }
    }
}
