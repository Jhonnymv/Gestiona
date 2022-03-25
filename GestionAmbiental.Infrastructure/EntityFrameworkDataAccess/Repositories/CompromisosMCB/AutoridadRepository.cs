using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class AutoridadRepository : IAutoridadReadOnlyRepository
    {
        private readonly Context _contex;

        public AutoridadRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarAutoridad(AutoridadCMCB input)
        {
            var entity = await _contex.AutoridadCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Autoridad = input.Autoridad;
            }
            _contex.AutoridadCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<AutoridadCMCB>> ListarAutoridades()
        {
            var autoridades = await _contex.AutoridadCMCB.ToListAsync();
            var ouput = new List<AutoridadCMCB>();
            foreach (var item in autoridades)
            {
                ouput.Add(new Domain.CompromisosMCB.AutoridadCMCB
                {
                    Id = item.Id,
                    Autoridad = item.Autoridad
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarAutoridad(AutoridadCMCB input)
        {
            var entity = new Entities.CompromisosMCB.AutoridadCMCB
            {
                Autoridad = input.Autoridad,
            };

            await _contex.AutoridadCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
