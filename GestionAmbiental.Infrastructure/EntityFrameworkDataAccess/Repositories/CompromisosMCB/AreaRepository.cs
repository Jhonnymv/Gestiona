using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class AreaRepository : IAreaReadOnlyRepository
    {
        private readonly Context _contex;

        public AreaRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarArea(AreaCMCB input)
        {
            var entity = await _contex.AreaCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Area = input.Area;
            }
            _contex.AreaCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<AreaCMCB>> ListarAreas()
        {
            var areas = await _contex.AreaCMCB.ToListAsync();
            var ouput = new List<AreaCMCB>();
            foreach (var item in areas)
            {
                ouput.Add(new Domain.CompromisosMCB.AreaCMCB
                {
                    Id = item.Id,
                    Area = item.Area
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarArea(AreaCMCB input)
        {
            var entity = new Entities.CompromisosMCB.AreaCMCB
            {
                Area = input.Area,
            };

            await _contex.AreaCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
