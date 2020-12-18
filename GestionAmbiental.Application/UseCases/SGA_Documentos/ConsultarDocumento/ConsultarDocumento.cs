using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento
{
    public class ConsultarDocumento : IConsultarDocumento
    {
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;

        public ConsultarDocumento(IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository)
        {
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
        }

        public async Task<DocumentoOutput> Execute(int IdDocumento)
        {
            var documento = await _getSGADocumentosReadOnlyRepository.GetDocumento(IdDocumento);
            var output = new DocumentoOutput(documento);
            return output;
        }
    }
}
