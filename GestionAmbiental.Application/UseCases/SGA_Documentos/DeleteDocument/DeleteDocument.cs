using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.DeleteDocument
{
    public class DeleteDocument : IDeleteDocument
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;

        public DeleteDocument(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
        }
        public async Task Execute(int IdDocument)
        {
            var document= new Documento();
            await _editarDocumentosReadOnlyRepository.DeleteDocument(IdDocument);

            var predecesor = new Documento();

        }
    }
}
