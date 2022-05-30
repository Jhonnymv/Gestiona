using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FaunaAsociadaAvistamientoRepository : IFaunaAsociadaReadOnlyRepository
    {
        private readonly Context _context;

        public FaunaAsociadaAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<faunaAsociadaAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var faunaAsociadas = await _context.FaunaAsociadaAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<faunaAsociadaAvistamiento>();

            foreach (var item in faunaAsociadas)
            {
                var dataFaunaAsociada = await _context.FaunaAsociadaMCIF.FindAsync(item.Id_FaunaAsociada);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new faunaAsociadaAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    FaunaAsociada = item.Id_FaunaAsociada != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada { Id = (int)item.Id_FaunaAsociada, Nombre = dataFaunaAsociada.Nombre, Descripcion = dataFaunaAsociada.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion=item.Descripcion
                });
                
            }
            return ouput;
        }

        public Task EditarFaunaAsociadaAvistamiento(faunaAsociadaAvistamiento faunaAsociadaAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<faunaAsociadaAvistamiento>> ListFaunaAsociadaAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarFaunaAsociadaAvistamiento(faunaAsociadaAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_FaunaAsociada = input.FaunaAsociada.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.FaunaAsociadaAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;

        }
    }
}
