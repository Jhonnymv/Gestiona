using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.AmpliarFechaRevision
{
    public class AmpliarFechaRevision : IAmpliarFechaRevision
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;

        public AmpliarFechaRevision(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
        }

        public async Task Execute(int IdRevision, string nuevaFecha)
        {
            var input = new RevisionDocumento
            {
                IdSGARevisionDocumento = IdRevision,
                FechaLimite = Convert.ToDateTime(nuevaFecha)
            };
            await _editarDocumentosReadOnlyRepository.AmpliarFechaRevision(input);
        }
    }
}
