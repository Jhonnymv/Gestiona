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

        public async Task<int> Execute(int categoriaTipo, string nombreDoc, string versionDoc, string comentario, string fechaTentativa, string fechaAprobacion, string rutaPdf,string rutaEditable, int idElaborador)
        {
            //var usuario = await _getUsuario.Execute(usuarioRed);

            var model = new Documento();
            model.SGACategoriaTipo = new CategoriaTipo
            {
                IdSGACategoriaTipo = categoriaTipo
            };
            model.DocCodigo = "TBD";
            model.DocNombre = nombreDoc;
            model.DocVersion = versionDoc;
            model.Comentario = comentario;
            model.PdfRuta = rutaPdf;
            model.EditableRuta = rutaEditable;
            model.Usuario = new Domain.ValueObjects.Usuario
            {
                IdUsuario = idElaborador
            };
            if (fechaTentativa != null)
            {
                model.FechaTentativa = Convert.ToDateTime(fechaTentativa);
            }
            if (fechaAprobacion != null)
            {
                model.FechaAprobacion = Convert.ToDateTime(fechaAprobacion);
            }
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
