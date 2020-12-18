using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos
{
    public class ConsultarDocumentos : IConsultarDocumentos
    {
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;

        public ConsultarDocumentos(IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository)
        {
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
        }

        public async Task<DocumentosOutput> Execute(string nombre, int tipo, string codigo, int condicion,int pagina, int rpp)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, tipo, codigo, condicion);
            data = data.OrderByDescending(x => Int32.Parse(x.DocCodigo.Remove(0, 2))).ToList();

            var output = new DocumentosOutput
            {
                Documentos = new List<DocumentoOutput>(),
                NRegistros = data.Count
            };

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                output.Documentos.Add(new DocumentoOutput(item));
            }
            return output;
        }

        public async Task<List<DocumentoOutput>> Execute()
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(null, 0, null, 0);

            var output = new List<DocumentoOutput>();

            //data = data.Where(x => x.Estado.Equals("Aprobado")).ToList();
            foreach (var item in data)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }
    }
}
