using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class UsoAvistamientoRepository : IUsoAvistamientoReadOnlyRepository
    {
        private readonly Context _context;

        public UsoAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<UsoAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var registros = await _context.UsoAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<UsoAvistamiento>();

            foreach (var item in registros)
            {
                var dataEntity = await _context.UsoMCIF.FindAsync(item.Id_Uso);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new UsoAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    Uso = item.Id_Uso != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso { Id = (int)item.Id_Uso, Nombre = dataEntity.Nombre, Descripcion = dataEntity.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion
                });

            }
            return ouput;
        }

        public Task EditarUsoAvistamiento(UsoAvistamiento usoAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsoAvistamiento>> ListUsoAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarUsoAvistamiento(UsoAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_Uso = input.Uso.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.UsoAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }
    }
}
