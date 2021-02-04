using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.FirmarDocumento
{
    public class FirmarDocumentos : IFirmarDocumento
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        public FirmarDocumentos(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
        }

        public async Task Execute(int IdDocumento, string DocFirmado)
        {
            var input = new Documento { 
                IdSGADocumento = IdDocumento,
                DocRuta = DocFirmado,
                Condicion = new Condicion
                {
                    IdSGACondicion = 6
                },
                Estado = EstadosDocumentosSGA.Publicado.ToString()
            };
            await _editarDocumentosReadOnlyRepository.FirmarDocumento(input);
        }
    }
}
