using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
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
            data.Fecha_Revision = DateTime.Now;

            _context.SGA_Revisiones_Documento.Update(data);
            await _context.SaveChangesAsync();
        }

        public async Task EditarEstadoDocumento(Documento documento)
        {
            var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(documento.IdSGADocumento));
            data.Condicion = documento.Condicion.IdSGACondicion;
            data.Estado = documento.Estado;

            _context.SGA_Documentos.Update(data);
            await _context.SaveChangesAsync();
        }
        public async Task EditarEstadoDocumento(int IdDocumento)
        {
            var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(IdDocumento));
            var primeraRevision = await _context.SGA_Revisiones_Documento.FirstAsync(x => x.Id_SGA_Tipo_Revision.Equals(1) && x.Orden.Equals(1));
            primeraRevision.Estado = EstadosRevisionDocSGA.Activo.ToString();

            _context.SGA_Revisiones_Documento.Update(primeraRevision);
            await _context.SaveChangesAsync();
        }
        public async Task RechazarDocumento(int IdDocumento)
        {
            var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(IdDocumento));
            data.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            data.Condicion = 1;

            var revisiones = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(IdDocumento)).ToListAsync();
            foreach(var item in revisiones)
            {
                item.Estado = EstadosRevisionDocSGA.Inactivo.ToString();
                item.Comentario = null;
                item.Fecha_Revision = null;
            }
            //revisiones.First(x => x.Id_SGA_Tipo_Revision.Equals(1) && x.Orden.Equals(1)).Estado = EstadosRevisionDocSGA.Activo.ToString();

            _context.SGA_Documentos.Update(data);
            _context.SGA_Revisiones_Documento.UpdateRange(revisiones);

            await _context.SaveChangesAsync();
        }
    }
}
