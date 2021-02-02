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

        public async Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int condicion,int pagina, int rpp)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, categoria, tipos, codigo, condicion);
            /*
            var dataNoTBD = data.Where(x=>!x.DocCodigo.Equals("TBD")).OrderByDescending(x => Int32.Parse(x.DocCodigo.Remove(0, 2))).ToList();
            //Buscar los Ids de documentos predecesores
            var predecesores = dataNoTBD.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = dataNoTBD.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).ToList();

            var dataTBD = data.Where(x=>x.DocCodigo.Equals("TBD")).OrderByDescending(x => x.FechaCreacion).ToList();
            data = dataTBD;
            data.AddRange(ultimasVersiones);
            */
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

        public async Task<List<DocumentoOutput>> ExecutePublicados()
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos();

            var output = new List<DocumentoOutput>();

            //Buscar los Ids de documentos predecesores
            var predecesores = data.Where(x=>x.DocPredecesor != null).Select(x=>x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = data.Where(x => !predecesores.Contains(x.IdSGADocumento) && (x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())|| x.Estado.Equals(EstadosDocumentosSGA.Obsoleto.ToString()))).ToList();

            foreach (var item in ultimasVersiones)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }
        public async Task<List<DocumentoOutput>> ExecutePredecesores(int Id)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos();
            var actual = data.First(x => x.IdSGADocumento.Equals(Id));

            var output = new List<DocumentoOutput>();

            //Buscar los Ids de documentos predecesores
            var predecesores = data.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = data.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).ToList();
            if (actual.DocPredecesor != null)
            {
                var predecesor = data.First(x => x.IdSGADocumento.Equals(actual.DocPredecesor.IdSGADocumento));
                ultimasVersiones.Add(predecesor);
            }
            foreach (var item in ultimasVersiones)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }
        public async Task<List<DocumentoOutput>> ExecuteSecundarios(int Id)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos();
            var actual = data.First(x => x.IdSGADocumento.Equals(Id));

            var output = new List<DocumentoOutput>();

            //Buscar los documentos secundarios (formatos)
            var secundarios = data.Where(x => x.DocPrincipal != null && x.DocPrincipal.IdSGADocumento.Equals(actual.IdSGADocumento)).ToList();
           
            foreach (var item in secundarios)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }

        public async Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int[] condicion, int pagina, int rpp)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, categoria, tipos, codigo, condicion);
            /*
            //var dataNoTBD = data.Where(x => !x.DocCodigo.Equals("TBD")).OrderByDescending(x => Int32.Parse(x.DocCodigo.Remove(0, 2))).ToList();
            var dataNoTBD = data.Where(x => !x.DocCodigo.Equals("TBD")).ToList();
            //Buscar los Ids de documentos predecesores
            var predecesores = dataNoTBD.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado
            var ultimasVersiones = dataNoTBD.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).ToList();

            var dataTBD = data.Where(x => x.DocCodigo.Equals("TBD")).OrderByDescending(x => x.FechaCreacion).ToList();
            data = dataTBD;
            
            data.AddRange(ultimasVersiones);
            */
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
    }
}
