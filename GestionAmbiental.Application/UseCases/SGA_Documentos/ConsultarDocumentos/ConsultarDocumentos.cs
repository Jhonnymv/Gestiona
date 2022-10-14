using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos
{
    public class ConsultarDocumentos : IConsultarDocumentos
    {
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public ConsultarDocumentos(IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository, IGetUsuario getUsuario)
        {
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int condicion,int pagina, int rpp)
        {            
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, categoria, tipos, codigo, condicion);
            //var predecesores = data.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            data = data
                .Where(x => !x.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()) &&
                !x.Estado.Equals(EstadosDocumentosSGA.Superado.ToString())
                //&& x.Acceso.Contains()
                )
                //&&!predecesores.Contains(x.IdSGADocumento))
                .OrderByDescending(x => x.FechaCreacion)
                .ToList();

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
        public async Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int condicion, int pagina, int rpp, string usuario)
        {
            var user = await _getUsuario.Execute(usuario);
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, categoria, tipos, codigo, condicion);
            //var predecesores = data.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            data = data
                .Where(x => !x.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()) &&
                !x.Estado.Equals(EstadosDocumentosSGA.Superado.ToString())
                //&& x.Acceso.Contains()
                )
                //&&!predecesores.Contains(x.IdSGADocumento))
                .OrderByDescending(x => x.FechaCreacion)
                .ToList();

            var output = new DocumentosOutput
            {
                Documentos = new List<DocumentoOutput>(),
                NRegistros = 0
            };

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {                
                if(item.Acceso!=null)
                {
                    if (item.Acceso.Split("|").Contains(user.IdUsuario.ToString()))
                        output.Documentos.Add(new DocumentoOutput(item));
                }
                else
                {
                    output.Documentos.Add(new DocumentoOutput(item));
                }
            }
            output.NRegistros = output.Documentos.Count;
            return output;
        }
        public async Task<List<DocumentoOutput>> ExecutePublicados()
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos();

            var output = new List<DocumentoOutput>();

            //Buscar los Ids de documentos predecesores
            var predecesores = data.Where(x => x.DocPredecesor != null).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            //var predecesores = data.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Publicado.ToString())).Select(x => x.DocPredecesor.IdSGADocumento).ToList();
            if (predecesores.Count > 0)
            {
                //Si no esta en la lista de predecesores, es la última versión y su estado es Publicado           
                var ultimasVersiones = data.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString())).ToList();
                foreach (var item in ultimasVersiones)
                {
                    output.Add(new DocumentoOutput(item));
                }
            }
            else
            {
                var ultimasVersiones = data.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString())).ToList();
                foreach (var item in ultimasVersiones)
                {
                    output.Add(new DocumentoOutput(item));
                }
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
            var ultimasVersiones = data.Where(x => !predecesores.Contains(x.IdSGADocumento) && x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString())).ToList();
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
            var data = await _getSGADocumentosReadOnlyRepository.GetSecundarios(Id);

            var output = new List<DocumentoOutput>();

            foreach (var item in data)
            {
                output.Add(new DocumentoOutput(item));
            }
            return output;
        }

        public async Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int[] condicion, int pagina, int rpp)
        {
            var data = await _getSGADocumentosReadOnlyRepository.GetDocumentos(nombre, categoria, tipos, codigo, condicion);
            data = data.OrderByDescending(x => x.FechaCreacion).ToList();
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
