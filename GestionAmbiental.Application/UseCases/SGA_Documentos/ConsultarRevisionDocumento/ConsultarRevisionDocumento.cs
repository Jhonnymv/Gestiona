using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarRevisionDocumento
{
    public class ConsultarRevisionDocumento : IConsultarRevisionDocumento
    {
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;

        public ConsultarRevisionDocumento(IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository)
        {
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
        }

        public async Task<RevisionAprobacionInputOutput> Execute(int IdRevisionDocumento)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetRevisionDocumento(IdRevisionDocumento);
            var output = new RevisionAprobacionInputOutput { 
                IdSGARevisionDocumento = data.IdSGARevisionDocumento,
                IdSGADocumento = data.IdSGADocumento,
                Comentario = data.Comentario,
                Estado = data.Estado,
                FechaLimite = data.FechaLimite,
                FechaRevision = data.FechaRevision,
                IdSGATipoRevision = data.SGATipoRevision.IdTipoRevision,
                Orden = data.Orden,
                Usuario = data.Usuario.IdUsuario
            };
            return output;
        }
    }
}
