using GestionAmbiental.Application.Repositories.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using System.Linq;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento
{
    public class EditarDocumento : IEditarDocumento
    {
        private readonly IEditarDocumentosReadOnlyRepository _editarDocumentosReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public EditarDocumento(IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IGetUsuario getUsuario)
        {
            _editarDocumentosReadOnlyRepository = editarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarDocumentosReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task Execute(int IdDoc, int IdCategoria, int IdCategoriaTipo, string codigo, string codigoTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario, string rutaDoc,string fechaTentativa, string usuarioRed, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores, List<int> secundarios)
        {
            var predecesor = new Documento();
            if (idPredecesor.HasValue && idPredecesor.Value != 0)
                predecesor.IdSGADocumento = idPredecesor.Value;

            var usuario = await _getUsuario.Execute(usuarioRed);
            
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
                DocRuta = rutaDoc,
                Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.IdUsuario
                },
                DocCodigo = "TBD"
            };

            //Actualizar listas de revisores y aprobadores antes de activar la primera revision
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
                    FechaLimite = Convert.ToDateTime(item.Fecha),
                    SGATipoRevision = new TipoRevision { IdTipoRevision = 1 },
                    Estado = item.Orden == 1 ? EstadosRevisionDocSGA.Activo.ToString() : EstadosRevisionDocSGA.Inactivo.ToString()
                });
            }
            await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listRevisiones, 1, model.IdSGADocumento);
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
            await _editarDocumentosReadOnlyRepository.EditarRevisionesDocumento(listAprobaciones, 2, model.IdSGADocumento);


            if (fechaTentativa != null)
            {
                model.FechaTentativa = Convert.ToDateTime(fechaTentativa);
            }
            if (codigo != null && codigo.Length > 0)
                model.DocCodigo = codigo;

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

            }
            if (secundarios.Count > 0)
            {
                model.Principal = true;
                await _editarDocumentosReadOnlyRepository.EditarDocumentosRelacionados(secundarios, model.IdSGADocumento);
            }

            await _editarDocumentosReadOnlyRepository.EditarDocumento(model);

            
        }
    }
}
