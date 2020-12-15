using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
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
    }
}
