using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
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
            var dataNoTBD = data.Where(x=>!x.DocCodigo.Equals("TBD")).OrderByDescending(x => Int32.Parse(x.DocCodigo.Remove(0, 2))).ToList();
            //Buscar los Ids de documentos predecesores
            var predecesores = dataNoTBD.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = dataNoTBD.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).ToList();

            var dataTBD = data.Where(x=>x.DocCodigo.Equals("TBD")).OrderByDescending(x => x.FechaCreacion).ToList();
            data = dataTBD;
            data.AddRange(ultimasVersiones);

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

            //Buscar los Ids de documentos predecesores
            var predecesores = data.Where(x=>x.DocPredecesor != null).Select(x=>x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = data.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).ToList();

            foreach (var item in ultimasVersiones)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }
    }
}
