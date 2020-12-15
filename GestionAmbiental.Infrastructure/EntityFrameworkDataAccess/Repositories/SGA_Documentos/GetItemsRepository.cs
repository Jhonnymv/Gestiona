using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Condicion>> GetCondiciones()
        {
            var data = await _context.SGA_Condiciones.ToListAsync();
            var output = new List<Condicion>();
            foreach (var item in data)
            {
                var outputItem = new Condicion
                {
                    IdSGACondicion = item.Id_SGA_Condicion,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<TipoDocumento>> GetTiposDocumento()
        {
            var data = await _context.SGA_Tipos_Documento.ToListAsync();
            var output = new List<TipoDocumento>();
            foreach (var item in data)
            {
                var outputItem = new TipoDocumento
                {
                    IdSGATipoDocumento = item.Id_SGA_Tipo_Documento,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
    }
}
