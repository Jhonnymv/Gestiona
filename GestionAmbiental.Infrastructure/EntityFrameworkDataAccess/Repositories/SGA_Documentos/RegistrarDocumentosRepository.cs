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
    public class RegistrarDocumentosRepository : IRegistrarDocumentosReadOnlyRepository
    {
        private readonly Context _context;

        public RegistrarDocumentosRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> RegistrarDocumento(Documento documento)
        {
            var entidad = new Entities.SGA_Documentos.Documento();
            entidad.Id_SGA_Tipo_Documento = documento.SGATipoDocumento.IdSGATipoDocumento;
            entidad.Doc_Codigo = documento.DocCodigo;
            entidad.Doc_Nombre = documento.DocNombre;
            entidad.Doc_Version = documento.DocVersion;
            if(documento.DocPredecesor.IdSGADocumento != 0)
                entidad.Doc_Predecesor = documento.DocPredecesor.IdSGADocumento;

            entidad.Fecha_Aprobacion = documento.FechaAprobacion;
            entidad.Condicion = documento.Condicion.IdSGACondicion;
            entidad.Comentario = documento.Comentario;
            entidad.Fecha_Tentativa = documento.FechaTentativa;
            entidad.Doc_Ruta = documento.DocRuta;
            entidad.Id_Usuario = documento.Usuario.IdUsuario;
            entidad.Fecha_Creacion = DateTime.Today;
            entidad.Fecha_Actualizacion = entidad.Fecha_Creacion;
            entidad.Estado = documento.Estado;

            _context.SGA_Documentos.Add(entidad);
            await _context.SaveChangesAsync();

            return entidad.Id_SGA_Documento;
        }

        public async Task<string> GetUltimoCodigo()
        {

            var codigo = await _context.SGA_Documentos.ToListAsync();
            if (codigo.Count > 0)
                return codigo.OrderBy(x => Int32.Parse(x.Doc_Codigo.Remove(0,2))).Select(x => x.Doc_Codigo.Remove(0, 2)).LastOrDefault();
            else
                return null;
        }

        public async Task<int> RegistrarRevisiones(List<RevisionDocumento> revisiones)
        {
            var data = new List<Entities.SGA_Documentos.Revision_Documento>();
            foreach(var item in revisiones)
            {
                data.Add(new Entities.SGA_Documentos.Revision_Documento
                {
                    Id_Usuario = item.IdUsuario,
                    Id_SGA_Tipo_Revision = item.SGATipoRevision.IdTipoRevision,
                    Id_SGA_Documento = item.IdSGADocumento,
                    Fecha_Limite = item.FechaLimite,
                    Orden = item.Orden,
                    Estado = item.Estado
                });
            }
            _context.SGA_Revisiones_Documento.AddRange(data);
            await _context.SaveChangesAsync();

            return data.Count;
        }
        public async Task<int> RegistrarRevision(RevisionDocumento revision)
        {
            var entity = new Entities.SGA_Documentos.Revision_Documento
            {
                Id_Usuario = revision.IdUsuario,
                Id_SGA_Tipo_Revision = revision.SGATipoRevision.IdTipoRevision,
                Id_SGA_Documento = revision.IdSGADocumento,
                Fecha_Limite = revision.FechaLimite,
                Orden = revision.Orden,
                Estado = revision.Estado
            };
            _context.SGA_Revisiones_Documento.Add(entity);
                
            await _context.SaveChangesAsync();

            return entity.Id_SGA_Revision_Documento;
        }
    }
}
