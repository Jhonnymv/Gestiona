using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class SaludAvistamientoRepository: ISaludAvistamientoReadOnlyRepository
    {
        private readonly Context _context;

        public SaludAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<SaludAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var registros = await _context.SaludAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<SaludAvistamiento>();

            foreach (var item in registros)
            {
                var dataEntity = await _context.SaludMCIF.FindAsync(item.Id_Salud);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new SaludAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    Salud = item.Id_Salud != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud { Id = (int)item.Id_Salud, Nombre = dataEntity.Nombre, Descripcion = dataEntity.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion
                });

            }
            return ouput;
        }

        public Task EditarSaludAvistamiento(SaludAvistamiento saludAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<SaludAvistamiento>> ListSaludAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarSaludAvistamiento(SaludAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_Salud = input.Salud.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.SaludAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }
    }
}
