using GestionAmbiental.Domain.SGA_Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.SGA_Documentos;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class GetUsuariosNotificarRepository : IGetUsuariosNotificarReadOnlyRepository
    {
        private readonly Context _context;

        public GetUsuariosNotificarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Usuarios_Notificar_SGA>> GetUsuariosNotificarSGA()
        {
            var output = new List<Usuarios_Notificar_SGA>();
            var usuarios = await _context.Usuarios_Notificar_SGA.ToListAsync();
            foreach (var usuario in usuarios)
            {
                output.Add(new Usuarios_Notificar_SGA
                {
                    Id_Usuarios_Notificar_SGA = usuario.Id_Usuarios_Notificar_SGA,
                    Id_Rol = usuario.Id_Rol,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Usuario_Red = usuario.usuario_red,
                    Email = usuario.Email,
                    Estado = usuario.Estado
                });
            }

            return output;
        }
    }
}
