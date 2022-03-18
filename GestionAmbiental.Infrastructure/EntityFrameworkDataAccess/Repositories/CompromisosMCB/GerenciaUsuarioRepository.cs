using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaUsuarioMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class GerenciaUsuarioRepository : IGerenciaUsuarioReadOnlyRepository
    {
        private readonly Context _context;

        public GerenciaUsuarioRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<GerenciaUsuarioCMCB> BuscarPorID(int Id)
        {
            throw new NotImplementedException();
        }

        public Task EditarGerenciaUsuario(GerenciaUsuarioCMCB input)
        {
            throw new NotImplementedException();
        }

        public Task EliminarGerenciaUsuario(int IdEliminado)
        {
            throw new NotImplementedException();
        }

        public Task<List<GerenciaUsuarioCMCB>> ListarGerenciaUsuario()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistraGerenciaUsuario(GerenciaUsuarioCMCB input)
        {
            var idd = 0;
            var gerUsuarios = await _context.GerenciaUsuarioCMCB.ToListAsync();
            if (gerUsuarios != null)
            {
                var gerenciaUsuario = gerUsuarios.FirstOrDefault(x => x.Id_Usuario.Equals(input.Usuario.IdUsuario) && x.Id_Gerencia.Equals(input.Gerencia.Id) && x.Estado.Equals("1"));
                if (gerenciaUsuario == null)
                {
                    var entity = new Entities.CompromisosMCB.GerenciaUsuarioCMCB
                    {
                        Id_Usuario = input.Usuario.IdUsuario,
                        Id_Gerencia = input.Gerencia.Id,
                        Estado = input.Estado,
                    };
                    await _context.GerenciaUsuarioCMCB.AddAsync(entity);
                    await _context.SaveChangesAsync();
                    idd = entity.Id;
                }
            }
            else
            {
                var entity = new Entities.CompromisosMCB.GerenciaUsuarioCMCB
                {
                    Id_Usuario = input.Usuario.IdUsuario,
                    Id_Gerencia = input.Gerencia.Id,
                    Estado = input.Estado,
                };
                await _context.GerenciaUsuarioCMCB.AddAsync(entity);
                await _context.SaveChangesAsync();
                idd = entity.Id;
            }
            return idd;
        }
    }
}
