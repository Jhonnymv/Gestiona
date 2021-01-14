using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento
{
    public class RegistrarDocumento : IRegistrarDocumento
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarDocumento(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository, IEditarDocumentosReadOnlyRepository editarDocumentosReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario,  string rutaDoc, string usuarioRed, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores)
        {
            /*
            DateTime? fechaTentativaDT = null;
            if (fechaTentativa!=null)
                fechaTentativaDT = Convert.ToDateTime(fechaTentativa);
            */
            var predecesor = new Documento();
            if (idPredecesor.HasValue && idPredecesor.Value!=0)
                predecesor.IdSGADocumento = idPredecesor.Value;

            var usuario = await _getUsuario.Execute(usuarioRed);

            var model = new Documento
            {
                SGATipoDocumento = new TipoDocumento
                {
                    IdSGATipoDocumento = IdTipoDoc
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
            if (opcion.Equals("GuardarEstado"))
            {        
                //Pendiente
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 1
                };
                model.Estado = EstadosDocumentosSGA.Pendiente.ToString();
            }
            else if (opcion.Equals("Registrar"))
            {
                /*
                var ultimoCodigo = await _registrarDocumentosReadOnlyRepository.GetUltimoCodigo();
                model.DocCodigo = ultimoCodigo != null ? "D-" + (Int32.Parse(ultimoCodigo) + 1).ToString().PadLeft(7, '0') : "D-0000001";
                */
                //Pendiente
                model.Condicion = new Condicion
                {
                    IdSGACondicion = 2
                };
                model.Estado = EstadosDocumentosSGA.Registrado.ToString();
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
