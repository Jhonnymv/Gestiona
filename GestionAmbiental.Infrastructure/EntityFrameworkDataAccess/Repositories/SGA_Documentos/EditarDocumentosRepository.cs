using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.SGA_Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class EditarDocumentosRepository : IEditarDocumentosReadOnlyRepository
    {
        private readonly Context _context;

        public EditarDocumentosRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task EditarEstadoDocumento(Documento documento)
        {
            var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(documento.IdSGADocumento));
            data.Condicion = documento.Condicion.IdSGACondicion;
            data.Estado = documento.Estado;
            data.Fecha_Actualizacion = DateTime.Now;

            _context.SGA_Documentos.Update(data);
            await _context.SaveChangesAsync();
        }
        public async Task EditarDocumento(Documento documento)
        {
            var entidad = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(documento.IdSGADocumento));

            entidad.Id_SGA_Categoria_Tipo = documento.SGACategoriaTipo.IdSGACategoriaTipo;
            entidad.Doc_Codigo = documento.DocCodigo;
            entidad.Doc_Nombre = documento.DocNombre;
            entidad.Principal = documento.Principal;
            entidad.Doc_Version = documento.DocVersion;
            if (documento.DocPredecesor.IdSGADocumento != 0)
                entidad.Doc_Predecesor = documento.DocPredecesor.IdSGADocumento;

            entidad.Fecha_Aprobacion = documento.FechaAprobacion;
            entidad.Condicion = documento.Condicion.IdSGACondicion;
            entidad.Comentario = documento.Comentario;
            entidad.Fecha_Tentativa = documento.FechaTentativa;
            entidad.Pdf_Ruta = documento.PdfRuta;
            entidad.Editable_Ruta = documento.EditableRuta;
            entidad.Fecha_Aprobacion = documento.FechaAprobacion;
            entidad.Fecha_Actualizacion = DateTime.Now;
            entidad.Estado = documento.Estado;
            entidad.Id_Usuario = documento.Usuario.IdUsuario;

            _context.SGA_Documentos.Update(entidad);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetUltimoCodigo(int IdCategoriaTipo)
        {
            var codigo = await _context.SGA_Documentos.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.Id_SGA_Categoria_Tipo.Equals(IdCategoriaTipo)).ToListAsync();
            if (codigo.Count > 0)
                return codigo.OrderBy(x => Int32.Parse(x.Doc_Codigo.Split(".")[0].Split("-")[3])).Select(x => x.Doc_Codigo.Split(".")[0].Split("-")[3]).LastOrDefault();
            else
                return null;
        }
        public async Task EditarDocumentosRelacionados(List<int> secundarios, int idDocumento)
        {
            var data = await _context.SGA_Documentos_Relacionados.Where(x => x.Id_Documento_Principal.Equals(idDocumento)).ToListAsync();

            var eliminados = data.Where(x => !secundarios.Contains(x.Id_Documento_Secundario)).ToList();
            var existentes = data.Where(x => secundarios.Contains(x.Id_Documento_Secundario)).ToList();

            var entidades = new List<Entities.SGA_Documentos.Documentos_Relacionados>();

            foreach (var id in secundarios)
            {
                //Agregar los que no pertencen a existentes o eliminados
                if (!eliminados.Select(x => x.Id_Documento_Secundario).Contains(id) &&
                    !existentes.Select(x => x.Id_Documento_Secundario).Contains(id))
                {
                    entidades.Add(new Entities.SGA_Documentos.Documentos_Relacionados
                    {
                        Id_Documento_Principal = idDocumento,
                        Id_Documento_Secundario = id,
                        Fecha_Relacion = DateTime.Now,
                        Estado = "ACT"
                    });
                }
            }

            _context.SGA_Documentos_Relacionados.RemoveRange(eliminados);

            await _context.SGA_Documentos_Relacionados.AddRangeAsync(entidades);

            await _context.SaveChangesAsync();
        }
        public async Task EditarDocumentosSecundarios(List<Documento> secundarios)
        {
            var data = await _context.SGA_Documentos.Where(x =>
                secundarios.Select(y => y.IdSGADocumento).Contains(x.Id_SGA_Documento))
               .ToListAsync();

            foreach (var item in data)
            {
                var model = secundarios.Find(x => x.IdSGADocumento.Equals(item.Id_SGA_Documento));
                item.Doc_Codigo = model.DocCodigo;
                item.Estado = model.Estado;
                item.Condicion = model.Condicion.IdSGACondicion;
                item.Fecha_Actualizacion = model.FechaActualizacion;
                item.Fecha_Aprobacion = model.FechaAprobacion;
            }

            _context.SGA_Documentos.UpdateRange(data);
            await _context.SaveChangesAsync();
        }
        //No se utiliza
        //public async Task ActivarPrimeraRevision(int IdDocumento)
        //{
        //    var primeraRevision = await _context.SGA_Revisiones_Documento.FirstAsync(x => x.Id_SGA_Documento.Equals(IdDocumento) && x.Id_SGA_Tipo_Revision.Equals(1) && x.Orden.Equals(1));
        //    primeraRevision.Estado = EstadosRevisionDocSGA.Activo.ToString();

        //    _context.SGA_Revisiones_Documento.Update(primeraRevision);
        //    await _context.SaveChangesAsync();
        //}
        public async Task EditarRevisionDocumento(RevisionDocumento revision)
        {
            var data = await _context.SGA_Revisiones_Documento.FirstAsync(x => x.Id_SGA_Revision_Documento.Equals(revision.IdSGARevisionDocumento));
            data.Comentario = revision.Comentario;
            data.Estado = revision.Estado;
            data.Fecha_Revision = DateTime.Now;

            _context.SGA_Revisiones_Documento.Update(data);
            await _context.SaveChangesAsync();
        }
        public async Task EditarRevisionesDocumento(List<RevisionDocumento> revision, int IdTipo, int IdDocumento)
        {
            var entities = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(IdDocumento) && x.Id_SGA_Tipo_Revision.Equals(IdTipo)).OrderBy(x => x.Orden).ToListAsync();
            var nuevos = new List<Entities.SGA_Documentos.Revision_Documento>();
            //Si se agregaron revisores
            if (entities.Count < revision.Count)
            {
                for (int i = entities.Count; i < revision.Count; i++)
                {
                    nuevos.Add(new Entities.SGA_Documentos.Revision_Documento
                    {
                        Orden = revision[i].Orden,
                        Id_Usuario = revision[i].Usuario.IdUsuario,
                        Fecha_Revision = revision[i].FechaRevision,
                        Id_SGA_Tipo_Revision = revision[i].SGATipoRevision.IdTipoRevision,
                        Id_SGA_Documento = revision[i].IdSGADocumento,
                        Estado = revision[i].Estado
                    });
                }
                _context.SGA_Revisiones_Documento.AddRange(nuevos);
            }
            for (int i = 0; i < entities.Count; i++)
            {
                //Buscar las revisiones existentes por orden
                if (revision.Count > i && entities[i].Orden == revision[i].Orden)
                {
                    entities[i].Fecha_Revision = revision[i].FechaRevision;
                    entities[i].Id_Usuario = revision[i].Usuario.IdUsuario;
                }
                else
                {
                    //Eliminar si no existe (hay menos revisores)
                    _context.SGA_Revisiones_Documento.Remove(entities[i]);
                    entities.Remove(entities[i]);
                }
            }
            _context.SGA_Revisiones_Documento.UpdateRange(entities);

            await _context.SaveChangesAsync();
        }
        //public async Task RechazarDocumento(int IdDocumento)
        //{
        //    var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(IdDocumento));
        //    data.Estado = EstadosDocumentosSGA.Pendiente.ToString();
        //    data.Condicion = 1;
        //    data.Fecha_Actualizacion = DateTime.Now;

        //    var revisiones = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(IdDocumento)).ToListAsync();
        //    foreach (var item in revisiones)
        //    {
        //        item.Estado = EstadosRevisionDocSGA.Inactivo.ToString();
        //        item.Comentario = null;
        //        item.Fecha_Revision = null;
        //    }
        //    //revisiones.First(x => x.Id_SGA_Tipo_Revision.Equals(1) && x.Orden.Equals(1)).Estado = EstadosRevisionDocSGA.Activo.ToString();

        //    _context.SGA_Documentos.Update(data);
        //    _context.SGA_Revisiones_Documento.UpdateRange(revisiones);

        //    await _context.SaveChangesAsync();
        //}
        public async Task FirmarDocumento(Documento documento)
        {
            //Editar documento actual
            var data = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(documento.IdSGADocumento));
            data.Fecha_Aprobacion = DateTime.Now;
            data.Fecha_Actualizacion = DateTime.Now;
            data.Pdf_Ruta = documento.PdfRuta;
            data.Editable_Ruta = documento.EditableRuta;
            data.Doc_Codigo = documento.DocCodigo;
            data.Condicion = documento.Condicion.IdSGACondicion;
            data.Estado = documento.Estado;

            _context.SGA_Documentos.Update(data);
            await _context.SaveChangesAsync();
            //Actualizar estado del predecesor
            if (data.Doc_Predecesor != null)
            {
                var predecesorDoc = new Documento
                {
                    IdSGADocumento = data.Doc_Predecesor.Value,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = 7
                    },
                    Estado = EstadosDocumentosSGA.Superado.ToString()
                };
                await EditarEstadoDocumento(predecesorDoc);
            }
        }
        public async Task DeleteDocument(int idDocument)
        {
            var document = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(idDocument));
            if (document.Doc_Predecesor != null)
            {
                var doc_predecesor = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(document.Doc_Predecesor));
                doc_predecesor.Estado = "Eliminado";

                if (doc_predecesor.Doc_Predecesor != null)
                {
                    var doc_predecesor2 = await _context.SGA_Documentos.FirstAsync(x => x.Id_SGA_Documento.Equals(doc_predecesor.Doc_Predecesor));
                    doc_predecesor2.Estado = "Eliminado";
                    _context.SGA_Documentos.UpdateRange(doc_predecesor2);
                    await _context.SaveChangesAsync();
                }
                _context.SGA_Documentos.UpdateRange(doc_predecesor);
                await _context.SaveChangesAsync();
            }
            document.Estado = "Eliminado";
            _context.SGA_Documentos.UpdateRange(document);
            await _context.SaveChangesAsync();


        }
        //public async Task AmpliarFechaRevision(RevisionDocumento revision)
        //{
        //    //Buscar las revision vencida
        //    var revisionVencida = await _context.SGA_Revisiones_Documento.FirstAsync(x => x.Id_SGA_Revision_Documento.Equals(revision.IdSGARevisionDocumento));            
        //    //Calcular la cantidad de dias agregados
        //    var difDias = revision.FechaLimite.Value.Subtract(revisionVencida.Fecha_Limite.Value).TotalDays;
        //    //actualizar fecha y estado
        //    revisionVencida.Fecha_Limite = revision.FechaLimite;
        //    revisionVencida.Estado = EstadosRevisionDocSGA.Activo.ToString();
        //    //Buscar las revisiones del documento inactivas
        //    var revisionesSgtes  = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(revisionVencida.Id_SGA_Documento) && x.Estado.Equals(EstadosRevisionDocSGA.Inactivo.ToString())).ToListAsync();
        //    //Modificar cada fecha de las siguientes revisiones
        //    foreach(var revisionSgte in revisionesSgtes)
        //    {
        //        //Sumar la candidad de dias agregados a la fecha de la revision vencida
        //        var nuevaFecha = revisionSgte.Fecha_Limite.Value.AddDays(difDias);
        //        revisionSgte.Fecha_Limite = nuevaFecha;
        //    }
        //    _context.SGA_Revisiones_Documento.Update(revisionVencida);
        //    _context.SGA_Revisiones_Documento.UpdateRange(revisionesSgtes);

        //    await _context.SaveChangesAsync();
        //}
    }
}
