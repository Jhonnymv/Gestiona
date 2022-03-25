using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.ComponenteCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class ComponenteRepository : IComponeteReadOnlyRepository
    {
        private readonly Context _contex;

        public ComponenteRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarComponente(ComponenteCMCB input)
        {
            var entity = await _contex.ComponenteCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Tipo = input.Tipo;
                entity.Componente = input.Componente;
                entity.Servicio = input.Servicio;
            }
            _contex.ComponenteCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<ComponenteCMCB>> ListarComponentes()
        {
            var componentes = await _contex.ComponenteCMCB.ToListAsync();
            var ouput = new List<ComponenteCMCB>();
            foreach (var item in componentes)
            {
                ouput.Add(new Domain.CompromisosMCB.ComponenteCMCB
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    Componente = item.Componente,
                    Servicio = item.Servicio,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarComponente(ComponenteCMCB input)
        {
            var entity = new Entities.CompromisosMCB.ComponenteCMCB
            {
                Tipo = input.Tipo,
                Componente = input.Componente,
                Servicio = input.Servicio,
            };

            await _contex.ComponenteCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
