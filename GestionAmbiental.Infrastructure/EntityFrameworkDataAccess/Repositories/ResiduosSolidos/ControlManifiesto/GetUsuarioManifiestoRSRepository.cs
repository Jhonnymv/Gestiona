using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlManifiesto
{
    public class GetUsuarioManifiestoRSRepository : IGetUsuarioManifiestoRSReadOnlyRepository
    {
        private readonly Context _context;

        public GetUsuarioManifiestoRSRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Usuario_Notificar_RS>> GetUsuariosNotificarRS()
        {
            var usuarios = await _context.Usuarios_Notificar_RS.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Usuario_Notificar_RS>();
            foreach (var item in usuarios)
            {
                output.Add(new Usuario_Notificar_RS(item.Id_Usuarios_Notificar_RS,item.Id_Rol,item.Nombres,item.Apellidos,item.usuario_red,item.Email,item.Estado));
            }
            return output;
        }
    }
}
