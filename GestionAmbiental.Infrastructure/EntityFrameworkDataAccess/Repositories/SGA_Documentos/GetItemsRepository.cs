using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;
        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Condicion>> GetCondiciones()
        {
            var data = await _context.SGA_Condiciones.ToListAsync();
            var output = new List<Condicion>();
            foreach (var item in data)
            {
                var outputItem = new Condicion
                {
                    IdSGACondicion = item.Id_SGA_Condicion,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<TipoDocumento>> GetTiposDocumento()
        {
            var data = await _context.SGA_Tipos_Documento.ToListAsync();
            var output = new List<TipoDocumento>();
            foreach (var item in data)
            {
                var outputItem = new TipoDocumento
                {
                    IdSGATipoDocumento = item.Id_SGA_Tipo_Documento,
                    Descripcion = item.Descripcion,
                    Codigo = item.Codigo,
                    Formato = item.Formato,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Usuario>> GetListUsuarios()
        {
            var output = new List<Usuario>();
            var items = await _context.Usuarios.Where(x => x.Estado.Equals("ACT") && (x.Nombres.Length >0 || x.Apellidos.Length > 0)).OrderBy(x=>x.Nombres).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Usuario
                {
                    IdUsuario = item.Id_Usuario,
                    Nombres = item.Nombres.ToUpper(),
                    Apellidos = item.Apellidos.ToUpper(),
                    Email = item.Email,
                    IdRol = item.Id_Rol,
                    UsuarioRed = item.usuario_red,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<TipoRevision>> GetTiposRevision()
        {
            var data = await _context.SGA_Tipos_Revision.ToListAsync();
            var output = new List<TipoRevision>();
            foreach (var item in data)
            {
                var outputItem = new TipoRevision
                {
                    IdTipoRevision = item.Id_SGA_Tipo_Revision,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<CategoriaDocumento>> GetCategoriasDocumento()
        {
            var data = await _context.SGA_Categoria_Documento.ToListAsync();
            var output = new List<CategoriaDocumento>();
            foreach (var item in data)
            {
                var outputItem = new CategoriaDocumento
                {
                    Id_SGA_Categoria_Documento = item.Id_SGA_Categoria_Documento,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<CategoriaTipo>> GetCategoriasTipo()
        {
            var data = await _context.SGA_Categoria_Tipo.ToListAsync();
            var categorias = await _context.SGA_Categoria_Documento.ToListAsync();
            var tipos = await _context.SGA_Tipos_Documento.ToListAsync();
            var output = new List<CategoriaTipo>();
            foreach (var item in data)
            {
                var categoria = categorias.Find(x => x.Id_SGA_Categoria_Documento.Equals(item.Id_SGA_Categoria_Documento));
                var tipo = tipos.Find(x => x.Id_SGA_Tipo_Documento.Equals(item.Id_SGA_Tipo_Documento));
                var outputItem = new CategoriaTipo
                {
                    IdSGACategoriaTipo = item.Id_SGA_Categoria_Tipo,
                    CategoriaDocumento = new CategoriaDocumento
                    {
                        Id_SGA_Categoria_Documento = categoria.Id_SGA_Categoria_Documento,
                        Descripcion = categoria.Descripcion,
                        Estado = categoria.Estado
                    },
                    TipoDocumento = new TipoDocumento
                    {
                        IdSGATipoDocumento  = tipo.Id_SGA_Tipo_Documento,
                        Descripcion = tipo.Descripcion,
                        Codigo = tipo.Codigo,
                        Formato = tipo.Formato,
                        Estado = tipo.Estado
                    },
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
    }
}
