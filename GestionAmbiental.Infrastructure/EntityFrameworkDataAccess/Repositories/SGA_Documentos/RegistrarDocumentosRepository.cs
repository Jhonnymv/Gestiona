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
            entidad.Id_SGA_Categoria_Tipo = documento.SGACategoriaTipo.IdSGACategoriaTipo;
            entidad.Doc_Codigo = documento.DocCodigo;
            entidad.Doc_Nombre = documento.DocNombre;
            entidad.Principal = documento.Principal;
            entidad.Doc_Version = documento.DocVersion;
            if (documento.DocPredecesor!=null && documento.DocPredecesor.IdSGADocumento != 0)
                entidad.Doc_Predecesor = documento.DocPredecesor.IdSGADocumento;

            entidad.Fecha_Aprobacion = documento.FechaAprobacion;
            entidad.Condicion = documento.Condicion.IdSGACondicion;
            entidad.Comentario = documento.Comentario;
            entidad.Fecha_Tentativa = documento.FechaTentativa;
            entidad.Pdf_Ruta = documento.PdfRuta;
            entidad.Editable_Ruta = documento.EditableRuta;
            entidad.Id_Usuario = documento.Usuario.IdUsuario;
            entidad.Fecha_Creacion = DateTime.Now;
            entidad.Fecha_Actualizacion = entidad.Fecha_Creacion;
            entidad.Estado = documento.Estado;           

            _context.SGA_Documentos.Add(entidad);
            await _context.SaveChangesAsync();
            return entidad.Id_SGA_Documento;
        }

        public async Task<string> GetUltimoCodigo()
        {

            var codigo = await _context.SGA_Documentos.Where(x=>!x.Doc_Codigo.Equals("TBD")).ToListAsync();
            if (codigo.Count > 0)
                return codigo.OrderBy(x => Int32.Parse(x.Doc_Codigo.Remove(0,2))).Select(x => x.Doc_Codigo.Remove(0, 2)).LastOrDefault();
            else
                return null;
        }
        public async Task RegistrarFormatos(List<int> idsFormato, int idDocumento)
        {
            var entidades = new List<Entities.SGA_Documentos.Documentos_Relacionados>();
            foreach (var id in idsFormato)
            {
                entidades.Add(new Entities.SGA_Documentos.Documentos_Relacionados
                {
                    Id_Documento_Principal = idDocumento,
                    Id_Documento_Secundario = id,
                    Fecha_Relacion = DateTime.Now,
                    Estado = "ACT"
                });
            }
            await _context.SGA_Documentos_Relacionados.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();
        }
        //No se utiliza
        public async Task<int> RegistrarRevisiones(List<RevisionDocumento> revisiones)
        {
            var data = new List<Entities.SGA_Documentos.Revision_Documento>();
            foreach(var item in revisiones)
            {
                data.Add(new Entities.SGA_Documentos.Revision_Documento
                {
                    Id_Usuario = item.Usuario.IdUsuario,
                    Id_SGA_Tipo_Revision = item.SGATipoRevision.IdTipoRevision,
                    Id_SGA_Documento = item.IdSGADocumento,
                    Fecha_Revision = item.FechaRevision,
                    Orden = item.Orden,
                    Comentario = item.Comentario,
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
                Id_Usuario = revision.Usuario.IdUsuario,
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
