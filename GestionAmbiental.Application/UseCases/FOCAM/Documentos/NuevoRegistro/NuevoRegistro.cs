using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Documento;

namespace GestionAmbiental.Application.UseCases.FOCAM.Documentos.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly IDocumentoReadOnlyRepository _documentoReadOnlyRepository;

        public NuevoRegistro(IDocumentoReadOnlyRepository documentoReadOnlyRepository)
        {
            _documentoReadOnlyRepository = documentoReadOnlyRepository;
        }

        public async Task<int> Execute(int Id_Tipo, string Codigo, string Titulo, string Version, DateTime FechaAprobacion, string Estado, string Comentarios, DateTime FechaTentativaVersion, string UrlDocumento)
        {
            var input = new Domain.FOCAM.Documento();
            if(Id_Tipo != 0)
            {
                input.Tipo = new Domain.FOCAM.Tipo { Id = Id_Tipo };
            }
            input.Codigo = Codigo;
            input.Titulo = Titulo;
            input.Version = Version;
            input.FechaAprovacion= FechaAprobacion;
            input.Estado = Estado;
            input.Comentarios = Comentarios;
            input.FechaTentaivaVersion= FechaTentativaVersion;
            input.UrlDocumento = UrlDocumento;

            return await _documentoReadOnlyRepository.RegistrarDocumento(input); 
        }
    }
}
