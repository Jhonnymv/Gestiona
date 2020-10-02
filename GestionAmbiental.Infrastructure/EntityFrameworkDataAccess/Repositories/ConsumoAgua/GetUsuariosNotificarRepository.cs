using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Domain.ConsumoAgua;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ConsumoAgua
{
    public class GetUsuariosNotificarRepository : IGetUsuariosNotificarReadOnlyRepository
    {
        private readonly Context _context;

        public GetUsuariosNotificarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<UsuarioNotificarConsumoAgua>> GetUsuariosNotificarConsumoAgua()
        {
            var usuarios = await _context.Usuarios_Notificar_Consumo_Agua.ToListAsync();
            var output = new List<UsuarioNotificarConsumoAgua>();
            foreach (var usuario in usuarios)
            {
                output.Add(new UsuarioNotificarConsumoAgua
                {
                    Id_Usuario_Notificar_Consumo_Agua = usuario.Id_Usuarios_Notificar_Consumo_Agua,
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
