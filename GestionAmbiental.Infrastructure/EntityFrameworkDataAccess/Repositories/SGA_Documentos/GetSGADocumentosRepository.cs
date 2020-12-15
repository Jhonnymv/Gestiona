using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class GetSGADocumentosRepository : IGetSGADocumentosReadOnlyRepository
    {
        private readonly Context _context;

        public GetSGADocumentosRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Documento> GetDocumento(int Id)
        {
            var data = await _context.SGA_Documentos.FindAsync(Id);

            var tipoDoc = await _context.SGA_Tipos_Documento.FindAsync(data.Id_SGA_Tipo_Documento);

            var docPredecesor = new Entities.SGA_Documentos.Documento();
            var predecesor = new Documento();

            var condicionDoc = await _context.SGA_Condiciones.FindAsync(data.Condicion);

            var usuario = await _context.Usuarios.FindAsync(data.Id_Usuario);

            if (data.Doc_Predecesor.HasValue) {
                docPredecesor = await _context.SGA_Documentos.FindAsync(data.Doc_Predecesor.Value);
                predecesor = new Documento
                {
                    IdSGADocumento = docPredecesor.Id_SGA_Documento,
                    DocCodigo = docPredecesor.Doc_Codigo,
                    DocNombre = docPredecesor.Doc_Nombre,
                    DocVersion = docPredecesor.Doc_Version,
                    DocRuta = docPredecesor.Doc_Ruta,
                    FechaCreacion = docPredecesor.Fecha_Creacion,
                    FechaActualizacion = docPredecesor.Fecha_Actualizacion,
                    Estado = docPredecesor.Estado
                };
            }

            var output = new Documento
            {
                IdSGADocumento = data.Id_SGA_Documento,
                SGATipoDocumento = new TipoDocumento
                {
                    IdSGATipoDocumento = tipoDoc.Id_SGA_Tipo_Documento,
                    Descripcion = tipoDoc.Descripcion,
                    Estado = tipoDoc.Estado
                },
                DocCodigo = data.Doc_Codigo,
                DocNombre = data.Doc_Nombre,
                DocVersion = data.Doc_Version,
                DocPredecesor = predecesor,
                FechaAprobacion = data.Fecha_Aprobacion,
                Condicion = new Condicion
                {
                    IdSGACondicion = condicionDoc.Id_SGA_Condicion,
                    Descripcion = condicionDoc.Descripcion,
                    Estado = condicionDoc.Estado
                },
                Comentario = data.Comentario,
                FechaTentativa = data.Fecha_Tentativa,
                DocRuta = data.Doc_Ruta,
                Usuario = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.Id_Usuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    DNI = usuario.DNI,
                    Email = usuario.Email,
                    UsuarioRed = usuario.usuario_red,
                    Puesto = usuario.Puesto,
                    Estado = usuario.Estado,
                    IdRol = usuario.Id_Rol
                },
                FechaCreacion = data.Fecha_Creacion,
                FechaActualizacion = data.Fecha_Actualizacion,
                Estado = data.Estado
            };
            return output;
        }

        public async Task<List<Documento>> GetDocumentos(string nombre, int tipo, string codigo, int condicion)
        {
            var output = new List<Documento>();

            var data = await _context.SGA_Documentos.ToListAsync();
            data = data.Where(x =>
            x.Doc_Nombre.Contains(nombre !=null ? nombre : x.Doc_Nombre) ||
            x.Id_SGA_Tipo_Documento.Equals(tipo != 0 ? tipo : x.Id_SGA_Tipo_Documento) ||
            x.Doc_Codigo.Contains(codigo != null ? codigo : x.Doc_Codigo) ||
            x.Condicion.Equals(condicion != 0 ? condicion : x.Condicion)).ToList();

            var tiposDoc = await _context.SGA_Tipos_Documento
                .Where(x =>x.Id_SGA_Tipo_Documento.Equals(tipo!=0?tipo:x.Id_SGA_Tipo_Documento))
                .ToListAsync();

            var condiciones = await _context.SGA_Condiciones
                .Where(x => x.Id_SGA_Condicion.Equals(condicion != 0 ? condicion : x.Id_SGA_Condicion))
                .ToListAsync();

            var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            foreach(var item in data)
            {
                var tipoDocModel = tiposDoc.First(x => x.Id_SGA_Tipo_Documento.Equals(item.Id_SGA_Tipo_Documento));
                var condicionModel = condiciones.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                var outputItem = new Documento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGATipoDocumento = new TipoDocumento
                    {
                        IdSGATipoDocumento = tipoDocModel.Id_SGA_Tipo_Documento,
                        Descripcion = tipoDocModel.Descripcion,
                        Estado = tipoDocModel.Estado
                    },
                    DocCodigo = item.Doc_Codigo,
                    DocNombre = item.Doc_Nombre,
                    DocVersion = item.Doc_Version,
                    //DocPredecesor
                    FechaAprobacion = item.Fecha_Aprobacion,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = condicionModel.Id_SGA_Condicion,
                        Descripcion = condicionModel.Descripcion,
                        Estado = condicionModel.Estado
                    },
                    Comentario = item.Comentario,
                    FechaTentativa = item.Fecha_Tentativa,
                    DocRuta = item.Doc_Ruta,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuario.Id_Usuario,
                        Nombres = usuario.Nombres,
                        Apellidos = usuario.Apellidos,
                        IdRol = usuario.Id_Rol,
                        Email = usuario.Email,
                        Estado = usuario.Estado
                    },
                    FechaCreacion = item.Fecha_Creacion,
                    FechaActualizacion = item.Fecha_Actualizacion,
                    Estado = item.Estado
                };

                output.Add(outputItem);                    
            }
            return output;
        }
    }
}
