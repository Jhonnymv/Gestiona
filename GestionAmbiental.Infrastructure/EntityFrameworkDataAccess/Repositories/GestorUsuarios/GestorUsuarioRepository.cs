using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (usuario.Id_Rol.Equals((int)InspeccionRol.Admin))
                    return InspeccionRol.Admin;
                else if (usuario.Id_Rol.Equals((int)InspeccionRol.AsesorMAP))
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
        public async Task ActualizarUsuarios(List<Usuario> usuarios)
        {
            var lista_usuarios = await _context.Usuarios.ToListAsync();
            lista_usuarios.ForEach(x => x.Estado = "INA");
            var temp = new List<Entities.Usuario>();
            foreach(var usuario in usuarios)
            {
                var entidad = lista_usuarios.FirstOrDefault(x => x.usuario_red.Equals(usuario.UsuarioRed));
                if(entidad != null)
                {
                    entidad.Nombres = usuario.Nombres ?? "";
                    entidad.usuario_red = usuario.UsuarioRed ?? "";
                    entidad.Apellidos = usuario.Apellidos ?? "";
                    entidad.Email = usuario.Email ?? "";
                    entidad.Estado = "ACT";
                    temp.Add(entidad);
                }
                else
                {
                    entidad = new Entities.Usuario
                    {
                        Id_Rol = (int)InspeccionRol.Inspector,
                        Nombres = usuario.Nombres ?? "",
                        usuario_red = usuario.UsuarioRed ?? "",
                        Apellidos = usuario.Apellidos ?? "",
                        Email = usuario.Email ?? "",
                        Estado = "ACT"
                    };
                    temp.Add(entidad);
                }
            }
            lista_usuarios.RemoveAll(x => temp.Select(y=>y.Id_Usuario).Contains(x.Id_Usuario));
            lista_usuarios.AddRange(temp);
            _context.Usuarios.UpdateRange(lista_usuarios);
            await _context.SaveChangesAsync();
        }

    }
}
