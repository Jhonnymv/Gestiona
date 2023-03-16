using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class EtapaRepository : IEtapaReadOnlyRepository
    {
        private readonly Context _contex;

        public EtapaRepository(Context context)
        {
            _contex = context;
        }
        public async Task<EtapaCMCB> DetalleEtapa(int Id)
        {
            var etapa = await _contex.EtapaCMCB.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            return new EtapaCMCB
            {
                Id = etapa.Id,
                Etapa = etapa.Etapa,
            };
        }
        public async Task EditarEtapa(EtapaCMCB input)
        {
            var entity = await _contex.EtapaCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Etapa = input.Etapa;
            }
            _contex.EtapaCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<EtapaCMCB>> ListarEtapas()
        {
            var etapas = await _contex.EtapaCMCB.ToListAsync();
            var ouput = new List<EtapaCMCB>();
            foreach (var item in etapas)
            {
                ouput.Add(new EtapaCMCB
                {
                    Id = item.Id,
                    Etapa = item.Etapa,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarEtapa(EtapaCMCB input)
        {
            var entity = new Entities.CompromisosMCB.EtapaCMCB
            {
                Etapa = input.Etapa,
            };

            await _contex.EtapaCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
