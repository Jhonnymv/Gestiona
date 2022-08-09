using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.CalidaAgua
{
    public class getItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public getItemsRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var UsuarioEntity = await _context.Usuarios.ToListAsync();
            var ouput = new List<Usuario>();
            foreach (var item in UsuarioEntity)
            {
                ouput.Add(new Usuario
                {
                    IdUsuario = item.Id_Usuario,
                    Nombres = item.Nombres + " " + item.Apellidos + " - " + item.DNI,
                });
            }
            return ouput;
        }
    }
}
