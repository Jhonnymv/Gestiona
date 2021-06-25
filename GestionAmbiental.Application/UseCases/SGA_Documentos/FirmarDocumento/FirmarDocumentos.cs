using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;
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
        private readonly IConsultarDocumentos _consultarDocumentos;

        public FirmarDocumentos(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IConsultarDocumentos consultarDocumentos)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _consultarDocumentos = consultarDocumentos ?? throw new ArgumentNullException(nameof(consultarDocumentos));
        }

        public async Task Execute(int IdDocumento, string DocFirmado, string CodigoTipo, string CodigoDoc, int IdCategoriaTipo)
        {
            var input = new Documento { 
                IdSGADocumento = IdDocumento,
                PdfRuta = DocFirmado,                
                Condicion = new Condicion
                {
                    IdSGACondicion = 6
                },
                DocCodigo = CodigoDoc,
                Estado = EstadosDocumentosSGA.Aprobado.ToString()
            };
            if (CodigoDoc == "TBD")
            {
                var ultimo = await _editarDocumentosReadOnlyRepository.GetUltimoCodigo(IdCategoriaTipo);
                if (ultimo != null)
                    input.DocCodigo = "JU-MAP-" + CodigoTipo + "-" + (Convert.ToInt32(ultimo) + 1).ToString().PadLeft(3, '0');
                else
                    input.DocCodigo = "JU-MAP-" + CodigoTipo + "-001";
            }
            await _editarDocumentosReadOnlyRepository.FirmarDocumento(input);

            //Buscar y actualizar secundarios no aprobados
            var secundarios = await _consultarDocumentos.ExecuteSecundarios(IdDocumento);
            if (secundarios.Count > 0)
            {
                var docsSecundarios = new List<Documento>();
                int docCount = 1;
                foreach (var item in secundarios)
                {
                    if (!item.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && !item.Estado.Equals(EstadosDocumentosSGA.Superado.ToString()))
                    {
                        var secundario = new Documento
                        {
                            DocCodigo = input.DocCodigo + ".F" + docCount.ToString().PadLeft(2, '0'),
                            Estado = EstadosDocumentosSGA.Aprobado.ToString(),
                            Condicion = new Condicion
                            {
                                IdSGACondicion = 6,
                            },

                            IdSGADocumento = item.IdSGADocumento,
                            FechaActualizacion = DateTime.Now,
                            FechaAprobacion = DateTime.Now,
                        };
                        docsSecundarios.Add(secundario);
                        docCount++;
                    }
                }
                await _editarDocumentosReadOnlyRepository.EditarDocumentosSecundarios(docsSecundarios);
            }
        }
    }
}
