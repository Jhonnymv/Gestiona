using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FenologiaAvistamientoRepository : IFenologiaAvistamientoReadOnlyRepository
    {
        private readonly Context _context;

        public FenologiaAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<FenologiaAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var registros = await _context.FenologiaAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<FenologiaAvistamiento>();

            foreach (var item in registros)
            {
                var dataEntity = await _context.FenologiaMCIF.FindAsync(item.Id_Fenologia);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new FenologiaAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    Fenologia = item.Id_Fenologia != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia { Id = (int)item.Id_Fenologia, Nombre = dataEntity.Nombre, Descripcion = dataEntity.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion
                });

            }
            return ouput;
        }

        public Task EditarFenologiaAsociadaAvistamiento(FenologiaAvistamiento fenologiaAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<FenologiaAvistamiento>> ListFenologiaAsociadaAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarFenologiaAsociadaAvistamiento(FenologiaAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_Fenologia = input.Fenologia.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.FenologiaAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }
    }
}
