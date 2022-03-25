using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AspectoCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class AspectoRepository : IAspectoReadOnlyRepository
    {
        private readonly Context _contex;

        public AspectoRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarAspecto(AspectoCMCB input)
        {
            var entity = await _contex.AspectoCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Aspecto = input.Aspecto;
            }
            _contex.AspectoCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<AspectoCMCB>> ListarAspectos()
        {
            var aspectos = await _contex.AspectoCMCB.ToListAsync();
            var ouput = new List<AspectoCMCB>();
            foreach (var item in aspectos)
            {
                ouput.Add(new Domain.CompromisosMCB.AspectoCMCB
                {
                    Id = item.Id,
                    Aspecto = item.Aspecto
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarAspecto(AspectoCMCB input)
        {
            var entity = new Entities.CompromisosMCB.AspectoCMCB
            {
                Aspecto = input.Aspecto,
            };

            await _contex.AspectoCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
