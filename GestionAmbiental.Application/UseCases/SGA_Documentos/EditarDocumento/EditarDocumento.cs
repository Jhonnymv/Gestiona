using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using System.Linq;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento
{
    public class EditarDocumento : IEditarDocumento
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;
        private readonly IConsultarDocumentos _consultarDocumentos;

        public EditarDocumento(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IConsultarDocumentos consultarDocumentos, IGetUsuario getUsuario)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _consultarDocumentos = consultarDocumentos ?? throw new ArgumentNullException(nameof(consultarDocumentos));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task Execute(int IdDoc, int IdCategoria, int IdCategoriaTipo, string codigo, string codigoTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario, string rutaPdf,string rutaEditable,string fechaTentativa, string fechaAprobacion, int IdElaboradoPor, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores, List<int> secundarios)
        {
            var predecesor = new Documento();
            if (idPredecesor.HasValue && idPredecesor.Value != 0)
                predecesor.IdSGADocumento = idPredecesor.Value;
                        
            var model = new Documento
            {
                IdSGADocumento = IdDoc,
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
                Usuario = new Domain.ValueObjects.Usuario { 
                IdUsuario = IdElaboradoPor
                },
                DocCodigo = "TBD"
            };

            ////Actualizar listas de revisores y aprobadores antes de activar la primera revision
            //var listRevisiones = new List<RevisionDocumento>();
            //foreach (var item in revisores)
            //{
            //    listRevisiones.Add(new RevisionDocumento
            //    {
            //        IdSGADocumento = IdDoc,
            //        Usuario = new Domain.ValueObjects.Usuario
            //        {
            //            IdUsuario = item.Usuario
            //        },
            //        Orden = item.Orden,
            //        FechaLimite = Convert.ToDateTime(item.Fecha),
            //        SGATipoRevision = new TipoRevision { IdTipoRevision = 1 },
            //        Estado = item.Orden == 1 ? EstadosRevisionDocSGA.Activo.ToString() : EstadosRevisionDocSGA.Inactivo.ToString()
            //    });
            //}
            //await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listRevisiones, 1, model.IdSGADocumento);
            ////Cargar aprobaciones
            //var listAprobaciones = new List<RevisionDocumento>();
            //foreach (var item in aprobadores)
            //{
            //    listAprobaciones.Add(new RevisionDocumento
            //    {
            //        IdSGADocumento = IdDoc,
            //        Usuario = new Domain.ValueObjects.Usuario
            //        {
            //            IdUsuario = item.Usuario
            //        },
            //        Orden = item.Orden,
            //        FechaLimite = Convert.ToDateTime(item.Fecha),
            //        SGATipoRevision = new TipoRevision { IdTipoRevision = 2 },
            //        Estado = EstadosRevisionDocSGA.Inactivo.ToString()
            //    });
            //}
            //await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listAprobaciones, 2, model.IdSGADocumento);
            if (fechaTentativa != null)
            {
                model.FechaTentativa = Convert.ToDateTime(fechaTentativa);
            }
            if (fechaAprobacion != null)
            {
                model.FechaAprobacion = Convert.ToDateTime(fechaAprobacion);
            }
            if (codigo != null && codigo.Length > 0 && !codigo.Equals("TBD"))
                model.DocCodigo = codigo;
            /*
            if (opcion.Equals("GuardarEstado") || opcion.Equals("AgregarFormato"))
            {
                //Pendiente
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 1
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            else if (opcion.Equals("Registrar")&& !(revisores.Count.Equals(0) && aprobadores.Count.Equals(0)))
            {
               //Registrado
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 2
                };
                model.Estado = EstadosDocumentosSGA.Registrado.ToString();

                await _editarDocumentosReadOnlyRepository.ActivarPrimeraRevision(IdDoc);

            }
            //Secundarios Pendientes
            else if (opcion.Equals("Registrar")&& revisores.Count.Equals(0) && aprobadores.Count.Equals(0) && IdCategoria.Equals(1))
            {                
                var ultimo = await _editarDocumentosReadOnlyRepository.GetUltimoCodigo(IdCategoriaTipo);
                if (ultimo != null)
                    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-" + (Convert.ToInt32(ultimo) + 1).ToString().PadLeft(3, '0');
                else
                    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-001";
                //Aprobado
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 6
                };
                model.Estado = EstadosDocumentosSGA.Aprobado.ToString();
            }
            else
            {
                //Publicar documentos SGI o Externos
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
            {
                model.Principal = true;
                await _editarDocumentosReadOnlyRepository.EditarDocumentosRelacionados(secundarios, model.IdSGADocumento);
            }
            else
            {
                model.Principal = false; 
                await _editarDocumentosReadOnlyRepository.EditarDocumentosRelacionados(secundarios, model.IdSGADocumento);
            }
            if (opcion.Equals("GuardarEstado"))
            {
                model.Condicion = new Condicion
                {
                    IdSGACondicion = (int)EstadosDocumentosSGA.Pendiente
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            else
            {
                //var ultimo = await _editarDocumentosReadOnlyRepository.GetUltimoCodigo(IdCategoriaTipo);
                //if (ultimo != null)
                //    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-" + (Convert.ToInt32(ultimo) + 1).ToString().PadLeft(3, '0');
                //else
                //    model.DocCodigo = "JU-MAP-" + codigoTipoDoc + "-001";
                model.FechaAprobacion = DateTime.Today;
                model.Condicion = new Condicion
                {
                    IdSGACondicion = (int)EstadosDocumentosSGA.Aprobado
                };
                model.Estado = EstadosDocumentosSGA.Aprobado.ToString();
            }
            await _editarDocumentosReadOnlyRepository.EditarDocumento(model);
            //Editar revisiones
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
            await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listRevisiones, 1,model.IdSGADocumento);
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
            await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listAprobaciones, 2, model.IdSGADocumento);

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
        }
    }
}
