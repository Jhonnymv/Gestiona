using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores
{
    public class ConsultarPredecesores : IConsultarPredecesores
    {
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;

        public ConsultarPredecesores(IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository)
        {
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
        }

        public async Task<List<DocumentoOutput>> Execute(int Id)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetPredecesores(Id);
            data = data.OrderByDescending(x => x.DocVersion).ToList();

            var output = new List<DocumentoOutput>();

            foreach (var item in data)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }
    }
}
