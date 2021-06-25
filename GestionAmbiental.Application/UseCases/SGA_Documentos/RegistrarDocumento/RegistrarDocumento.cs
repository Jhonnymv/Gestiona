using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento
{
    public class RegistrarDocumento : IRegistrarDocumento
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository;
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        private readonly IConsultarDocumentos _consultarDocumentos;
        private readonly IGetUsuario _getUsuario;

        public RegistrarDocumento(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository, IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository,IConsultarDocumentos consultarDocumentos, IGetUsuario getUsuario)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _consultarDocumentos = consultarDocumentos ?? throw new ArgumentNullException(nameof(_consultarDocumentos));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdCategoriaTipo, string codigo, int IdDocPrincipal, string codigoTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario, string rutaPdf,string rutaEditable, string fechaTentativa, string fechaAprobacion, int IdElaboradoPor, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores, List<int>secundarios)
        {
            var predecesor = new Documento();
            if (idPredecesor.HasValue && idPredecesor.Value!=0)
                predecesor.IdSGADocumento = idPredecesor.Value;

            //var usuario = await _getUsuario.Execute(usuarioRed);
                        
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
                PdfRuta = rutaPdf,
                EditableRuta = rutaEditable,
                Principal = false,
                Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = IdElaboradoPor
                },
                DocCodigo = "TBD"
            };
            if (fechaTentativa != null)
            {
                model.FechaTentativa = Convert.ToDateTime(fechaTentativa);
            }
            if (fechaAprobacion != null)
            {
                model.FechaAprobacion = Convert.ToDateTime(fechaAprobacion);
            }
            if (opcion.Equals("GuardarEstado"))
            {        
                //Pendiente
                model.Condicion = new Condicion
                {
                    IdSGACondicion = (int)EstadosDocumentosSGA.Pendiente
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            else if (opcion.Equals("Registrar") && IdDocPrincipal == 0)
            {
                //var ultimo = await _editarDocumentosReadOnlyRepository.GetUltimoCodigo(IdCategoriaTipo);
                //if (ultimo != null)
                //    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-" + (Convert.ToInt32(ultimo) + 1).ToString().PadLeft(3, '0');
                //else
                //    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-001";
                //Registrado
                model.Condicion = new Condicion
                {
                    IdSGACondicion = (int)EstadosDocumentosSGA.Aprobado
                };
                model.FechaAprobacion = DateTime.Today;
                model.Estado = EstadosDocumentosSGA.Aprobado.ToString();
            }

            if(codigo!= null && codigo.Length > 0)
                model.DocCodigo = codigo;

            if (IdDocPrincipal != 0)
            {
                model.Condicion = new Condicion
                {
                    IdSGACondicion = (int)EstadosDocumentosSGA.Pendiente
                };
                model.Principal = false;
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            //Si es un documento sin revision directamente se registra como publicado
            /*if(revisores.Count.Equals(0) && aprobadores.Count.Equals(0) && opcion.Equals("Registrar"))
            {
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 6
                };
                model.Estado = EstadosDocumentosSGA.Aprobado.ToString();
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
                        Estado = EstadosDocumentosSGA.Superado.ToString()
                    };
                    await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(predecesorDoc);
                }
            }*/
            if (secundarios.Count > 0)
                model.Principal = true;

            var IdDoc =  await _registrarDocumentosReadOnlyRepository.RegistrarDocumento(model);            
            await _registrarDocumentosReadOnlyRepository.RegistrarFormatos(secundarios,IdDoc);

            if (opcion.Equals("Registrar"))
            {
                if (idPredecesor.HasValue && idPredecesor.Value != 0)
                {
                    var predecesorDoc = new Documento
                    {
                        IdSGADocumento = idPredecesor.Value,
                        Condicion = new Condicion
                        {
                            IdSGACondicion = (int)EstadosDocumentosSGA.Superado
                        },
                        Estado = EstadosDocumentosSGA.Superado.ToString()
                    };
                    await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(predecesorDoc);
                }
                var secundariosTBD = await _consultarDocumentos.ExecuteSecundarios(IdDoc);
                if (secundariosTBD.Count > 0)
                {
                    var docsSecundarios = new List<Documento>();
                    int docCount = 1;
                    foreach (var item in secundariosTBD)
                    {
                        if (!item.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && !item.Estado.Equals(EstadosDocumentosSGA.Superado.ToString()))
                        {
                            var secundario = new Documento
                            {
                                DocCodigo = model.DocCodigo + ".F" + docCount.ToString().PadLeft(2, '0'),
                                Estado = EstadosDocumentosSGA.Aprobado.ToString(),
                                Condicion = new Condicion
                                {
                                    IdSGACondicion = (int)EstadosDocumentosSGA.Aprobado,
                                },

                                IdSGADocumento = item.IdSGADocumento,
                                FechaActualizacion = DateTime.Now,
                                FechaAprobacion = DateTime.Now,
                            };
                            docsSecundarios.Add(secundario);
                            docCount++;
                        }
                    }
                    await _editarDocumentosReadOnlyRepository.EditarDocumentosSecundarios(docsSecundarios);
                }
            }
           
            //Cargar revisiones
            var listRevisiones = new List<RevisionDocumento>();
            foreach (var item in revisores)
            {
                listRevisiones.Add(new RevisionDocumento
                {
                    IdSGADocumento = IdDoc,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = item.Usuario
                    },
                    Orden = item.Orden,
                    FechaRevision = Convert.ToDateTime(item.Fecha),
                    SGATipoRevision = new TipoRevision { IdTipoRevision = 1 },
                    Estado = EstadosRevisionDocSGA.Registrado.ToString()
                    //Estado = item.Orden==1? EstadosRevisionDocSGA.Activo.ToString(): EstadosRevisionDocSGA.Inactivo.ToString()
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
                    FechaRevision = Convert.ToDateTime(item.Fecha),
                    SGATipoRevision = new TipoRevision { IdTipoRevision = 2 },
                    Estado = EstadosRevisionDocSGA.Registrado.ToString()
                    //Estado = EstadosRevisionDocSGA.Inactivo.ToString()
                });
            }
            await _registrarDocumentosReadOnlyRepository.RegistrarRevisiones(listAprobaciones);
            
            return IdDoc;
        }
    }
}
