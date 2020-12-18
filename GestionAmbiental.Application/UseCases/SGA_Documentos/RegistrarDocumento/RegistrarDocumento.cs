using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento
{
    public class RegistrarDocumento : IRegistrarDocumento
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository;

        public RegistrarDocumento(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
        }

        public async Task<int> Execute(int IdTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario,  string rutaDoc, string usuarioRed)
        {
            /*
            DateTime? fechaTentativaDT = null;
            if (fechaTentativa!=null)
                fechaTentativaDT = Convert.ToDateTime(fechaTentativa);
            */
            var predecesor = new Documento();
            if (idPredecesor.HasValue)
                predecesor.IdSGADocumento = idPredecesor.Value;

            var ultimoCodigo = await _registrarDocumentosReadOnlyRepository.GetUltimoCodigo();
           
            var model = new Documento
            {
                SGATipoDocumento = new TipoDocumento
                {
                    IdSGATipoDocumento = IdTipoDoc
                },
                DocCodigo = ultimoCodigo!=null? "D-"+(Int32.Parse(ultimoCodigo) + 1).ToString().PadLeft(7,'0'): "D-0000001",
                DocNombre = nombreDoc,
                DocVersion = versionDoc,
                DocPredecesor = predecesor,
                Condicion = new Condicion { 
                    IdSGACondicion = 1
                },
                Comentario = comentario,
                DocRuta = rutaDoc,
                Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = 1
                },
                Estado = "Registrado"
            };

            return await _registrarDocumentosReadOnlyRepository.RegistrarDocumento(model);
        }
    }
}
