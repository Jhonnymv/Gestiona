using GestionAmbiental.Application.Repositories.Incidentes.Items;
using GestionAmbiental.Domain.Incidentes.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.Items
{
    public class GetUsuariosNotificarRepositoy : IGetUsuariosNotificarReadOnlyRepository
    {
        private readonly Context _context;

        public GetUsuariosNotificarRepositoy(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Usuario_Notificar_Incidente>> GetUsuariosNotificarIncidente()
        {
            var usuarios = await _context.Usuario_Notificar_Incidente.ToListAsync();
            var output = new List<Usuario_Notificar_Incidente>();
            foreach(var usuario in usuarios)
            {
                var outputItem = new Usuario_Notificar_Incidente();
                outputItem.Id_Usuario_Notificar_Incidente = usuario.Id_Usuarios_Notificar_Incidente;
                outputItem.Id_Rol=usuario.Id_Rol;
                outputItem.Nombres = usuario.Nombres;
                outputItem.Apellidos = usuario.Apellidos;
                outputItem.Usuario_Red = usuario.usuario_red;
                outputItem.Email = usuario.Email;
                outputItem.Estado = usuario.Estado;
                output.Add(outputItem);
            }
            return output;
        }
    }
}
