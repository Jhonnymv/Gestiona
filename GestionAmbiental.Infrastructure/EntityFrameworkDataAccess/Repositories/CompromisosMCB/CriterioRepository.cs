using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class CriterioRepository : ICriterioReadOnlyRepository
    {
        private readonly Context _contex;

        public CriterioRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarCriterio(CriterioCMCB input)
        {
            var entity = await _contex.CriterioCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Criterio = input.Criterio;
                entity.Detalle = input.Detalle;
            }
            _contex.CriterioCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<CriterioCMCB>> ListarCriterios()
        {
            var criterios = await _contex.CriterioCMCB.ToListAsync();
            var ouput = new List<CriterioCMCB>();
            foreach (var item in criterios)
            {
                ouput.Add(new Domain.CompromisosMCB.CriterioCMCB
                {
                    Id = item.Id,
                    Criterio = item.Criterio,
                    Detalle = item.Detalle,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarCriterio(CriterioCMCB input)
        {
            var entity = new Entities.CompromisosMCB.CriterioCMCB
            {
                Criterio = input.Criterio,
                Detalle=input.Detalle,
            };

            await _contex.CriterioCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
