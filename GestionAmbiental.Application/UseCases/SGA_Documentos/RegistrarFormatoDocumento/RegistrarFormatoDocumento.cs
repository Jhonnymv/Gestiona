using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarFormatoDocumento
{
    public class RegistrarFormatoDocumento : IRegistrarFormatoDocumento
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository; 
        private readonly IGetUsuario _getUsuario;

        public RegistrarFormatoDocumento(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int categoriaTipo, string nombreDoc, double versionDoc, string comentario, string rutaDoc, string usuarioRed)
        {
            var usuario = await _getUsuario.Execute(usuarioRed);

            var model = new Documento();
            model.SGACategoriaTipo = new CategoriaTipo
            {
                IdSGACategoriaTipo = categoriaTipo
            };
            model.DocCodigo = "TBD";
            model.DocNombre = nombreDoc;
            model.DocVersion = versionDoc;
            model.Comentario = comentario;
            model.DocRuta = rutaDoc;
            model.Usuario = new Domain.ValueObjects.Usuario
            {
                IdUsuario = usuario.IdUsuario
            };

            model.Condicion = new Condicion
            {
                IdSGACondicion = 1
            };
            model.Principal = false;
            model.Estado = EstadosDocumentosSGA.Pendiente.ToString();

            return await _registrarDocumentosReadOnlyRepository.RegistrarDocumento(model);

        }
    }
}
