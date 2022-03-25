using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.OrigenCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class OrigenRepository : IOrigenReadOnlyRepository
    {
        private readonly Context _contex;

        public OrigenRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarOrigen(OrigenCMCB input)
        {
            var entity = await _contex.OrigenCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Origen = input.Origen;
            }
            _contex.OrigenCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<OrigenCMCB>> ListarOrigenes()
        {
            var etapas = await _contex.OrigenCMCB.ToListAsync();
            var ouput = new List<OrigenCMCB>();
            foreach (var item in etapas)
            {
                ouput.Add(new OrigenCMCB
                {
                    Id = item.Id,
                    Origen = item.Origen,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarOrigen(OrigenCMCB input)
        {
            var entity = new Entities.CompromisosMCB.OrigenCMCB
            {
                Origen =input.Origen,
            };

            await _contex.OrigenCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
