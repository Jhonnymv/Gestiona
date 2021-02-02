using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento
{
    public class RegistrarDocumento : IRegistrarDocumento
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository;
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarDocumento(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository, IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdCategoriaTipo,string codigo, int IdDocPrincipal, string nombreDoc, double versionDoc, int? idPredecesor, string comentario,  string rutaDoc, string usuarioRed, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores)
        {
            var predecesor = new Documento();
            if (idPredecesor.HasValue && idPredecesor.Value!=0)
                predecesor.IdSGADocumento = idPredecesor.Value;

            var usuario = await _getUsuario.Execute(usuarioRed);

            var model = new Documento
            {
                SGACategoriaTipo = new CategoriaTipo
                {
                    IdSGACategoriaTipo = IdCategoriaTipo
                },               
                DocNombre = nombreDoc,
                DocVersion = versionDoc,
                DocPredecesor = predecesor,
                Comentario = comentario,
                DocRuta = rutaDoc,
                Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.IdUsuario
                },
                DocCodigo = "TBD"
            };
            if (opcion.Equals("GuardarEstado") || opcion.Equals("AgregarFormato"))
            {        
                //Pendiente
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 1
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            else if (opcion.Equals("Registrar") && IdDocPrincipal == 0)
            {
                //Registrado
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 2
                };
                model.Estado = EstadosDocumentosSGA.Registrado.ToString();
            }

            if(codigo!= null && codigo.Length > 0)
                model.DocCodigo = codigo;

            if (IdDocPrincipal != 0)
            {
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 1
                };
                model.DocPrincipal = new Documento
                {
                    IdSGADocumento = IdDocPrincipal
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            //Si es un documento sin revision directamente se registra como publicado
            if(revisores.Count.Equals(0) && aprobadores.Count.Equals(0) && opcion.Equals("Registrar"))
            {
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 6
                };
                model.Estado = EstadosDocumentosSGA.Publicado.ToString();
                //Hacer obsoleto el predecesor si existe
                if (idPredecesor.HasValue && idPredecesor.Value != 0)
                {
                    var predecesorDoc = new Documento
                    {
                        IdSGADocumento = idPredecesor.Value,
                        Condicion = new Condicion
                        {
                            IdSGACondicion = 7
                        },
                        Estado = EstadosDocumentosSGA.Obsoleto.ToString()
                    };
                    await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(predecesorDoc);
                }
            }

            var IdDoc =  await _registrarDocumentosReadOnlyRepository.RegistrarDocumento(model);
            
            //Cargar revisiones
            var listRevisiones = new List<RevisionDocumento>();
            foreach(var item in revisores)
            {
                listRevisiones.Add(new RevisionDocumento
                {
                    IdSGADocumento = IdDoc,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = item.Usuario
                    },
                    Orden = item.Orden,
                    FechaLimite = Convert.ToDateTime(item.Fecha),
                    SGATipoRevision = new TipoRevision { IdTipoRevision = 1},
                    Estado = item.Orden==1? EstadosRevisionDocSGA.Activo.ToString(): EstadosRevisionDocSGA.Inactivo.ToString()
                });
            }
            await _registrarDocumentosReadOnlyRepository.RegistrarRevisiones(listRevisiones);
            //Cargar aprobaciones
            var listAprobaciones = new List<RevisionDocumento>();
            foreach (var item in aprobadores)
            {
                listAprobaciones.Add(new RevisionDocumento
                {
                    IdSGADocumento = IdDoc,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = item.Usuario
                    },
                    Orden = item.Orden,
                    FechaLimite = Convert.ToDateTime(item.Fecha),
                    SGATipoRevision = new TipoRevision { IdTipoRevision = 2 },
                    Estado = EstadosRevisionDocSGA.Inactivo.ToString()
                });
            }
            await _registrarDocumentosReadOnlyRepository.RegistrarRevisiones(listAprobaciones);

            return IdDoc;
        }
    }
}
