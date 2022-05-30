using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class VigorAvistamientoRepository : IVigorAvistamientoReadOnlyRepository
    {
        private readonly Context _context;

        public VigorAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<VigorAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var registros = await _context.VigorAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<VigorAvistamiento>();

            foreach (var item in registros)
            {
                var dataEntity = await _context.VigorMCIF.FindAsync(item.Id_Vigor);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new VigorAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    Vigor = item.Id_Vigor != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor { Id = (int)item.Id_Vigor, Nombre = dataEntity.Nombre, Descripcion = dataEntity.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion
                });

            }
            return ouput;
        }

        public Task EditarVigorAvistamiento(VigorAvistamiento vigorAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<VigorAvistamiento>> ListVigorAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarVigorAvistamiento(VigorAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_Vigor = input.Vigor.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.VigorAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }
    }
}
