using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class NombreCientificoAvistamientoRepository:INombreCientificoAvistamientoReadOnlyRepository
    {
        private readonly Context _context;

        public NombreCientificoAvistamientoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<NombreCientificoAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento)
        {
            var nombresCientficos = await _context.NombreFientificoAvistamientoMCIF.Where(x => x.Id_Avistamiento == IdAvistamiento).ToListAsync();

            var ouput = new List<NombreCientificoAvistamiento>();

            foreach (var item in nombresCientficos)
            {
                var datanombreCeintifico = await _context.NombreFientificoMCIF.FindAsync(item.Id_NombreCientifico);
                var Avistamiento = await _context.AvistamientoFloraMCIF.FindAsync(item.Id_Avistamiento);

                ouput.Add(new NombreCientificoAvistamiento
                {
                    Id = item.Id,
                    Avistamiento = item.Id_Avistamiento != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora { Id = (int)item.Id_Avistamiento } : null,
                    NombreCientifico = item.Id_NombreCientifico != null ? new Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico { Id = (int)item.Id_NombreCientifico, Nombre = datanombreCeintifico.Nombre,NombreComun=datanombreCeintifico.NombreComun, Descripcion = datanombreCeintifico.Descripcion } : null,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion
                });

            }
            return ouput;
        }

        public Task EditarNombreCientificoAvistamiento(NombreCientificoAvistamiento nombreCientificoAvistamiento)
        {
            throw new NotImplementedException();
        }

        public Task<List<NombreCientificoAvistamiento>> ListNombreCientificoAvistamiento()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegistrarNombreCientificoaAvistamiento(NombreCientificoAvistamiento input)
        {
            var Entity = new Entities.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento();
            Entity.Id_Avistamiento = input.Avistamiento.Id;
            Entity.Id_NombreCientifico = input.NombreCientifico.Id;
            Entity.Cantidad = input.Cantidad;
            Entity.Descripcion = input.Descripcion;

            _context.NombreFientificoAvistamientoMCIF.Add(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }
    }
}
