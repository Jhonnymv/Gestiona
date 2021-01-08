using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class EditarDocumentosRepository : IEditarDocumentosReadOnlyRepository
    {
        private Context _context;

        public EditarDocumentosRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task EditarRevisionDocumento(RevisionDocumento revision)
        {
            var data = await _context.SGA_Revisiones_Documento.FirstAsync(x => x.Id_SGA_Revision_Documento.Equals(revision.IdSGARevisionDocumento));
            data.Comentario = revision.Comentario;
            data.Estado = revision.Estado;

            _context.SGA_Revisiones_Documento.Update(data);
            await _context.SaveChangesAsync();
        }
    }
}
