using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.AvistamientoFauna.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.AvistamientoFauna
{
    public class GetUsuariosNotificarRepository : IGetUsuariosNotificarReadOnlyRepository
    {
        private readonly Context _context;

        public GetUsuariosNotificarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Usuario_Notificar_Avistamiento_Fauna>> GetUsuariosNotificarIncidente()
        {
            var usuarios = await _context.usuario_Notificar_Avistamiento_Fauna.ToListAsync();
            var output = new List<Usuario_Notificar_Avistamiento_Fauna>();
            foreach (var usuario in usuarios)
            {
                output.Add(new Usuario_Notificar_Avistamiento_Fauna
                {
                    Id_Usuario_Notificar_Avistamiento_Fauna = usuario.Id_Usuarios_Notificar_Avistamiento_Fauna,
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
