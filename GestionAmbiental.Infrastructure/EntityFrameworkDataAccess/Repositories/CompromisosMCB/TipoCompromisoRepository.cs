using GestionAmbiental.Application.Repositories.CompromisosMCB.TipoCompromisoCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class TipoCompromisoRepository:ITipoCompromisoReadOnlyRepository
    {
        private readonly Context _context;

        public TipoCompromisoRepository(Context context)
        {
            _context = context;
        }

        public Task EditarTipoCompromiso(TipoCompromisoCMCB tipoCompromisoCMCB)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TipoCompromisoCMCB>> ListarTipoCompromisos()
        {
            var TipoCompromisos = await _context.TipoCompromisosCMCB.ToListAsync();
            var output=new List<TipoCompromisoCMCB>();

            foreach (var item in TipoCompromisos)
            {
                output.Add(new Domain.CompromisosMCB.TipoCompromisoCMCB
                {
                    Id= item.Id,
                    Tipo=item.Tipo,
                    Descripcion=item.Descripcion,
                });
            }
            return output;
        }

        public Task<int> RegistrarTipoCompromiso(TipoCompromisoCMCB tipoCompromisoCMCB)
        {
            throw new NotImplementedException();
        }
    }
}
