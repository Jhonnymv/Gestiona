using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Documento;
using GestionAmbiental.Domain.FOCAM;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.FOCAM
{
    public class DocumentoRepository:IDocumentoReadOnlyRepository
    {
        private readonly Context _context;

        public DocumentoRepository(Context context)
        {
            _context = context;
        }

        public async Task<Documento> BuscarPorID(int Id)
        {
            var data=await _context.DocumentoFOCAM.FindAsync(Id);
            var dataTipo=await _context.TipoFOCAM.FindAsync(data.Id_Tipo);
            var ouput = new Domain.FOCAM.Documento
            {
                Id = data.Id,
                Tipo = data.Id_Tipo != null ? new Domain.FOCAM.Tipo { Id = (int)data.Id_Tipo, Nombre = dataTipo.Nombre, Descripcion = dataTipo.Descripcion } : null,
                Codigo = data.Codigo,
                Titulo = data.Titulo,
                Version = data.Version,
                FechaAprovacion = data.FechaAprobacion,
                Estado = data.Estado,
                Comentarios = data.Comentarios,
                FechaTentaivaVersion = data.FechaTentaivaVersion,
                UrlDocumento = data.UrlDocumento
            };
            return ouput;
        }

        public Task EditarDocumento(Documento documento)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Documento>> ListDocuemntos()
        {
            var dataDocumentos = await _context.DocumentoFOCAM.ToListAsync();

            var ouput = new List<Documento>();
            foreach (var documento in dataDocumentos)
            {
                var dataTipo=await _context.TipoFOCAM.FindAsync(documento.Id_Tipo);
                ouput.Add(new Domain.FOCAM.Documento
                {
                    Id = documento.Id,
                    Tipo = documento.Id_Tipo != null ? new Domain.FOCAM.Tipo { Id = (int)documento.Id_Tipo, Nombre = dataTipo.Nombre, Descripcion = dataTipo.Descripcion } : null,
                    Codigo = documento.Codigo,
                    Titulo = documento.Titulo,
                    Version = documento.Version,
                    FechaAprovacion=documento.FechaAprobacion,
                    Estado = documento.Estado,
                    Comentarios = documento.Comentarios,
                    FechaTentaivaVersion = documento.FechaTentaivaVersion,
                    UrlDocumento = documento.UrlDocumento
                });
            }
            return ouput;
        }

        public async Task<int> RegistrarDocumento(Documento documento)
        {
            var Entity = new Entities.FOCAM.Documentos();

            Entity.Id_Tipo = documento.Tipo.Id;
            Entity.Codigo = documento.Codigo;
            Entity.Titulo = documento.Titulo;
            Entity.Version = documento.Version;
            Entity.FechaAprobacion = documento.FechaAprovacion;
            Entity.Estado = documento.Estado;
            Entity.Comentarios = documento.Comentarios;
            Entity.FechaTentaivaVersion = documento.FechaTentaivaVersion;
            Entity.UrlDocumento = documento.UrlDocumento;

            _context.DocumentoFOCAM.Add(Entity);
            await _context.SaveChangesAsync();

            return Entity.Id;
        }
    }
}
