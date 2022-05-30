using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Documento;

namespace GestionAmbiental.Application.UseCases.FOCAM.Documentos.ListarRegistros
{
    public class ListarRegistros: IListarRegistros
    {
        private readonly IDocumentoReadOnlyRepository _documentoReadOnlyRepository;

        public ListarRegistros(IDocumentoReadOnlyRepository documentoReadOnlyRepository)
        {
            _documentoReadOnlyRepository = documentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(documentoReadOnlyRepository));
        }

        public async Task<DocumentoOuput> BuscarById(int idDocumento)
        {
            var documento = await _documentoReadOnlyRepository.BuscarPorID(idDocumento);
            var ouput= new DocumentoOuput();

            ouput.Id=documento.Id;
            if (documento.Tipo!=null)
            {
                ouput.IdTipo=documento.Tipo.Id;
                ouput.NombreTipo=documento.Tipo.Nombre;
            }
            ouput.Codigo=documento.Codigo;
            ouput.Titulo=documento.Titulo;
            ouput.Version=documento.Version;
            ouput.FechaAprobacion = documento.FechaAprovacion;
            ouput.Estado=documento.Estado;
            ouput.Comentarios=documento.Comentarios;
            ouput.FechaTentaivaVersion=documento.FechaTentaivaVersion;
            ouput.UrlDocumento=documento.UrlDocumento;
            return ouput;
        }

        public async Task<DocumentosOuput> Execute(int pagina, int rpp)
        {
            var registros = await _documentoReadOnlyRepository.ListDocuemntos();
            var ouput = new DocumentosOuput();
            ouput.Registros = new List<DocumentoOuput>();
            foreach (var item in registros)
            {
                var DocuItem=new DocumentoOuput();
                DocuItem.Id=item.Id;
                if (item.Tipo!=null)
                {
                    DocuItem.IdTipo=item.Tipo.Id;
                    DocuItem.NombreTipo = item.Tipo.Nombre;
                }
                DocuItem.Codigo=item.Codigo;
                DocuItem.Titulo=item.Titulo;
                DocuItem.Version=item.Version;
                DocuItem.FechaAprobacion=item.FechaAprovacion;
                DocuItem.Estado=item.Estado;
                DocuItem.Comentarios=item.Comentarios;
                DocuItem.FechaTentaivaVersion=item.FechaTentaivaVersion;
                DocuItem.UrlDocumento=item.UrlDocumento;
                ouput.Registros.Add(DocuItem);
            }
            ouput.NRegistros = ouput.Registros.Count;
            ouput.Registros=ouput.Registros.Skip((pagina -1)*rpp).Take(rpp).ToList();
            return ouput;
        }
    }
}
