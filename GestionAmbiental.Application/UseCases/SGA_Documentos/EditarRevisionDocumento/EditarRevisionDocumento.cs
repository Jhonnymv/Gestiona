using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento
{
    public class EditarRevisionDocumento : IEditarRevisionDocumento
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        private readonly IGetSGADocumentosReadOnlyRepository _getSGADocumentosReadOnlyRepository;

        public EditarRevisionDocumento(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IGetSGADocumentosReadOnlyRepository getSGADocumentosReadOnlyRepository)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _getSGADocumentosReadOnlyRepository = getSGADocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(getSGADocumentosReadOnlyRepository));
        }

        public async Task Execute(int IdDocumento, int IdRevisionDocumento, string Comentario, string Opcion)
        {
            var input = new RevisionDocumento
            {
                IdSGARevisionDocumento = IdRevisionDocumento,
                Comentario = Comentario,
                Estado = Opcion.Equals("Aceptar")? EstadosRevisionDocSGA.Aprobado.ToString(): EstadosRevisionDocSGA.Rechazado.ToString()
            };
            await _editarDocumentosReadOnlyRepository.EditarRevisionDocumento(input);
            //Buscar documento siguiente
            var revisionesDocumento = await _getSGADocumentosReadOnlyRepository.GetRevisionesDocumento(IdDocumento);
           
            var revisionActual = revisionesDocumento.First(x => x.IdSGARevisionDocumento.Equals(IdRevisionDocumento));
            
            if (Opcion.Equals("Aceptar"))
            { 
                //Si se acepto el doc, cambiar estado a Activo la siguiente revision
                if (revisionActual.SGATipoRevision.IdTipoRevision.Equals(1))
                {
                    //Si esta en revision buscar siguiente
                    var siguienteRevision = revisionesDocumento.FirstOrDefault(x => x.SGATipoRevision.IdTipoRevision.Equals(1) && x.Orden.Equals(revisionActual.Orden + 1));
                    var primeraAprobacion = revisionesDocumento.FirstOrDefault(x => x.SGATipoRevision.IdTipoRevision.Equals(2) && x.Orden.Equals(1));
                    if (siguienteRevision != null)
                    {
                        //Si hay un siguiente del tipo Revision, cambiar a estado Activo a la siguiente del tipo Revision
                        var inputSiguiente = new RevisionDocumento
                        {
                            IdSGARevisionDocumento = siguienteRevision.IdSGARevisionDocumento,
                            Estado = EstadosRevisionDocSGA.Activo.ToString()
                        };
                        await _editarDocumentosReadOnlyRepository.EditarRevisionDocumento(inputSiguiente);
                        //cambiar estado del doc a En Revision
                        await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(
                            new Documento { 
                                IdSGADocumento = IdDocumento,
                                Condicion = new Condicion { IdSGACondicion = (int)EstadosDocumentosSGA.EnRevision },
                                Estado = EstadosDocumentosSGA.EnRevision.ToString() 
                            });
                    }
                    else
                    {
                        //Si no hay un siguiente del tipo Revision, cambiar a estado Activo a la primera del tipo Aprobacion
                        var inputSiguiente = new RevisionDocumento
                        {
                            IdSGARevisionDocumento = primeraAprobacion.IdSGARevisionDocumento,
                            Estado = EstadosRevisionDocSGA.Activo.ToString()
                        };
                        await _editarDocumentosReadOnlyRepository.EditarRevisionDocumento(inputSiguiente);
                        //cambiar estado del doc a En Aprobacion
                        await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(
                            new Documento
                            {
                                IdSGADocumento = IdDocumento,
                                Condicion = new Condicion { IdSGACondicion = (int)EstadosDocumentosSGA.EnAprobacion },
                                Estado = EstadosDocumentosSGA.EnAprobacion.ToString()
                            });
                    }
                }
                else if(revisionActual.SGATipoRevision.IdTipoRevision.Equals(2))
                {
                    //Si esta en aprobacion buscar siguiente
                    var siguienteAprobacion = revisionesDocumento.FirstOrDefault(x => x.SGATipoRevision.IdTipoRevision.Equals(2) && x.Orden.Equals(revisionActual.Orden + 1));
                    if (siguienteAprobacion != null)
                    {
                        //Si hay un siguiente del tipo Aprobacion, cambiar a estado Activo a la siguiente del tipo Aprobacion
                        var inputSiguiente = new RevisionDocumento
                        {
                            IdSGARevisionDocumento = siguienteAprobacion.IdSGARevisionDocumento,
                            Estado = EstadosRevisionDocSGA.Activo.ToString()
                        };
                        await _editarDocumentosReadOnlyRepository.EditarRevisionDocumento(inputSiguiente);
                        //cambiar estado del doc a En Revision
                        await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(
                           new Documento
                           {
                               IdSGADocumento = IdDocumento,
                               Condicion = new Condicion { IdSGACondicion = (int)EstadosDocumentosSGA.EnRevision },
                               Estado = EstadosDocumentosSGA.EnRevision.ToString()
                           });
                    }
                    else
                    {
                        //Si no hay un siguiente del tipo Aprobacion, cambiar a estado del Doc a Firmas
                        await _editarDocumentosReadOnlyRepository.EditarEstadoDocumento(
                           new Documento
                           {
                               IdSGADocumento = IdDocumento,
                               Condicion = new Condicion { IdSGACondicion = (int)EstadosDocumentosSGA.Firmas },
                               Estado = EstadosDocumentosSGA.Firmas.ToString()
                           });
                    }
                }
            }
            else
            {
                //Si se rechazo, cambiar todo a Inactivo (el primero a Activo), eliminar todos los comentario
                await _editarDocumentosReadOnlyRepository.RechazarDocumento(IdDocumento);
            }
           
        }
    }
}
