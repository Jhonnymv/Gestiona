using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class GerenciaRepository : IGerenciaReadOnlyRepository
    {
        private readonly Context _contex;

        public GerenciaRepository(Context context)
        {
            _contex = context;
        }

        public async Task EditarGerencia(GerenciaCMCB input)
        {
            var entity = await _contex.GerenciaCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Gerencia = input.Gerencia;
                entity.Descripcion = input.Descripcion;
            }
            _contex.GerenciaCMCB.Update(entity);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<GerenciaCMCB>> ListarGerencias()
        {
            var datos = await _contex.GerenciaCMCB.ToListAsync();
            var ouput = new List<GerenciaCMCB>();
            foreach (var item in datos)
            {
                ouput.Add(new GerenciaCMCB
                {
                    Id = item.Id,
                    Gerencia = item.Gerencia,
                    Descripcion = item.Descripcion,
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarGerencia(GerenciaCMCB input)
        {
            var entity = new Entities.CompromisosMCB.GerenciaCMCB
            {
                Gerencia =input.Gerencia,
                Descripcion=input.Descripcion,
            };

            await _contex.GerenciaCMCB.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity.Id;
        }
    }
}
