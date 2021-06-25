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

            var revisiones = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(Id)).ToListAsync();
            var usuarios = await _context.Usuarios.Where(x => revisiones.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            var categoriaTipoDoc = await _context.SGA_Categoria_Tipo.FindAsync(data.Id_SGA_Categoria_Tipo);
            var tipoDoc = await _context.SGA_Tipos_Documento.FindAsync(categoriaTipoDoc.Id_SGA_Tipo_Documento);
            var categoriaDoc = await _context.SGA_Categoria_Documento.FindAsync(categoriaTipoDoc.Id_SGA_Categoria_Documento);

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
                    Principal = docPredecesor.Principal,
                    PdfRuta = docPredecesor.Pdf_Ruta,
                    EditableRuta = docPredecesor.Editable_Ruta,
                    FechaCreacion = docPredecesor.Fecha_Creacion,
                    FechaActualizacion = docPredecesor.Fecha_Actualizacion,
                    Estado = docPredecesor.Estado
                };
            }

            var listRevisores = new List<RevisionDocumento>();
            foreach (var item in revisiones.Where(x => x.Id_SGA_Tipo_Revision.Equals(1)))
            {
                var usuarioItem = usuarios.Find(x => x.Id_Usuario.Equals(item.Id_Usuario));
                listRevisores.Add(new RevisionDocumento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGATipoRevision = new TipoRevision { IdTipoRevision = item.Id_SGA_Tipo_Revision },
                    IdSGARevisionDocumento = item.Id_SGA_Revision_Documento,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuarioItem.Id_Usuario,
                        Nombres = usuarioItem.Nombres.Replace('\'', ' '),
                        Apellidos = usuarioItem.Apellidos.Replace('\'', ' '),
                        Email = usuarioItem.Email
                    },
                    Orden = item.Orden,
                    FechaLimite = item.Fecha_Limite,
                    FechaRevision = item.Fecha_Revision,
                    Estado = item.Estado
                });
            }

            var listAprobadores = new List<RevisionDocumento>();
            foreach (var item in revisiones.Where(x => x.Id_SGA_Tipo_Revision.Equals(2)))
            {
                var usuarioItem = usuarios.Find(x => x.Id_Usuario.Equals(item.Id_Usuario));
                listAprobadores.Add(new RevisionDocumento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGATipoRevision = new TipoRevision { IdTipoRevision = item.Id_SGA_Tipo_Revision },
                    IdSGARevisionDocumento = item.Id_SGA_Revision_Documento,
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuarioItem.Id_Usuario,
                        Nombres = usuarioItem.Nombres.Replace('\'', ' '),
                        Apellidos = usuarioItem.Apellidos.Replace('\'', ' '),
                        Email = usuarioItem.Email
                    },
                    Orden = item.Orden,
                    FechaLimite = item.Fecha_Limite,
                    FechaRevision = item.Fecha_Revision,
                    Estado = item.Estado
                });
            }

            var output = new Documento
            {
                IdSGADocumento = data.Id_SGA_Documento,
                SGACategoriaTipo = new CategoriaTipo
                {
                    IdSGACategoriaTipo = categoriaTipoDoc.Id_SGA_Categoria_Tipo,
                    CategoriaDocumento = new CategoriaDocumento
                    {
                        Id_SGA_Categoria_Documento = categoriaDoc.Id_SGA_Categoria_Documento,
                        Descripcion = categoriaDoc.Descripcion,
                        Estado = categoriaTipoDoc.Estado
                    },
                    TipoDocumento = new TipoDocumento
                    {
                        IdSGATipoDocumento = tipoDoc.Id_SGA_Tipo_Documento,
                        Descripcion = tipoDoc.Descripcion,
                        Estado = tipoDoc.Estado,
                        Codigo = tipoDoc.Codigo,
                        Formato = tipoDoc.Formato
                    },
                    Estado = categoriaTipoDoc.Estado
                },
                DocCodigo = data.Doc_Codigo,
                DocNombre = data.Doc_Nombre,
                DocVersion = data.Doc_Version,
                DocPredecesor = predecesor,
                Principal = data.Principal,
                FechaAprobacion = data.Fecha_Aprobacion,
                Condicion = new Condicion
                {
                    IdSGACondicion = condicionDoc.Id_SGA_Condicion,
                    Descripcion = condicionDoc.Descripcion,
                    Estado = condicionDoc.Estado
                },
                Comentario = data.Comentario,
                FechaTentativa = data.Fecha_Tentativa,
                PdfRuta = data.Pdf_Ruta,
                EditableRuta = data.Editable_Ruta,
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
                Estado = data.Estado,
                Revisiones = listRevisores,
                Aprobaciones = listAprobadores
            };
            return output;
        }
        public async Task<List<Documento>> GetDocumentos(string nombre, int categoria, int[] tipos, string codigo, int condicion)
        {
            var output = new List<Documento>();

            var data = await _context.SGA_Documentos.ToListAsync();
            data = data.Where(x =>
            x.Doc_Nombre.ToLower().Contains(nombre != null ? nombre.ToLower() : x.Doc_Nombre.ToLower()) &&
            x.Doc_Codigo.ToLower().Contains(codigo != null ? codigo.ToLower() : x.Doc_Codigo.ToLower()) &&
            x.Condicion.Equals(condicion != 0 ? condicion : x.Condicion)).ToList();

            var tiposCategoria = await _context.SGA_Categoria_Tipo.Where(x => x.Id_SGA_Categoria_Documento.Equals(categoria)).ToListAsync();
            var categoriasData = await _context.SGA_Categoria_Documento.ToListAsync();
            var tiposData = await _context.SGA_Tipos_Documento.ToListAsync();

            if (tipos.Length > 0)
            {
                tiposCategoria = tiposCategoria.Where(x => tipos.Contains(x.Id_SGA_Tipo_Documento)).ToList();
            }
            data = data.Where(x => tiposCategoria.Select(y => y.Id_SGA_Categoria_Tipo).Contains(x.Id_SGA_Categoria_Tipo)).ToList();

            var condiciones = await _context.SGA_Condiciones
                .Where(x => x.Id_SGA_Condicion.Equals(condicion != 0 ? condicion : x.Id_SGA_Condicion))
                .ToListAsync();

            var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            foreach (var item in data)
            {
                var tipoCategoria = tiposCategoria.First(x => x.Id_SGA_Categoria_Tipo.Equals(item.Id_SGA_Categoria_Tipo));
                var tipoItem = tiposData.First(x => x.Id_SGA_Tipo_Documento.Equals(tipoCategoria.Id_SGA_Tipo_Documento));
                var categoriaItem = categoriasData.First(x => x.Id_SGA_Categoria_Documento.Equals(tipoCategoria.Id_SGA_Categoria_Documento));
                var condicionModel = condiciones.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                var outputItem = new Documento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGACategoriaTipo = new CategoriaTipo
                    {
                        IdSGACategoriaTipo = tipoCategoria.Id_SGA_Categoria_Tipo,
                        TipoDocumento = new TipoDocumento
                        {
                            IdSGATipoDocumento = tipoItem.Id_SGA_Tipo_Documento,
                            Descripcion = tipoItem.Descripcion,
                            Codigo = tipoItem.Codigo,
                            Formato = tipoItem.Formato,
                            Estado = tipoItem.Estado
                        },
                        CategoriaDocumento = new CategoriaDocumento
                        {
                            Id_SGA_Categoria_Documento = categoriaItem.Id_SGA_Categoria_Documento,
                            Descripcion = categoriaItem.Descripcion,
                            Estado = categoriaItem.Estado
                        },
                        Estado = tipoCategoria.Estado
                    },
                    DocCodigo = item.Doc_Codigo,
                    DocNombre = item.Doc_Nombre,
                    Principal = item.Principal,
                    DocVersion = item.Doc_Version,
                    FechaAprobacion = item.Fecha_Aprobacion,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = condicionModel.Id_SGA_Condicion,
                        Descripcion = condicionModel.Descripcion,
                        Estado = condicionModel.Estado
                    },
                    Comentario = item.Comentario,
                    FechaTentativa = item.Fecha_Tentativa,
                    PdfRuta = item.Pdf_Ruta,
                    EditableRuta = item.Editable_Ruta,
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
                if (item.Doc_Predecesor.HasValue)
                {
                    outputItem.DocPredecesor = new Documento
                    {
                        IdSGADocumento = item.Doc_Predecesor.Value
                    };
                }
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Documento>> GetDocumentos(string nombre, int categoria, int[] tipos, string codigo, int[] condiciones)
        {
            var output = new List<Documento>();

            var data = await _context.SGA_Documentos.ToListAsync();
            data = data.Where(x =>
            x.Doc_Nombre.ToLower().Contains(nombre != null ? nombre.ToLower() : x.Doc_Nombre.ToLower()) &&
            x.Doc_Codigo.ToLower().Contains(codigo != null ? codigo.ToLower() : x.Doc_Codigo.ToLower())).ToList();

            var tiposCategoria = await _context.SGA_Categoria_Tipo.Where(x => x.Id_SGA_Categoria_Documento.Equals(categoria)).ToListAsync();
            var categoriasData = await _context.SGA_Categoria_Documento.ToListAsync();
            var tiposData = await _context.SGA_Tipos_Documento.ToListAsync();

            if (tipos.Length > 0)
            {
                tiposCategoria = tiposCategoria.Where(x => tipos.Contains(x.Id_SGA_Tipo_Documento)).ToList();
            }
            data = data.Where(x => tiposCategoria.Select(y => y.Id_SGA_Categoria_Tipo).Contains(x.Id_SGA_Categoria_Tipo)).ToList();

            var condicionesEntities = await _context.SGA_Condiciones.ToListAsync();
            if (condiciones.Length > 0)
            {
                condicionesEntities = condicionesEntities.Where(x => condiciones.Contains(x.Id_SGA_Condicion)).ToList();
            }
            data = data.Where(x => condicionesEntities.Select(y => y.Id_SGA_Condicion).Contains(x.Condicion)).ToList();

            var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            foreach (var item in data)
            {
                var tipoCategoria = tiposCategoria.First(x => x.Id_SGA_Categoria_Tipo.Equals(item.Id_SGA_Categoria_Tipo));
                var tipoItem = tiposData.First(x => x.Id_SGA_Tipo_Documento.Equals(tipoCategoria.Id_SGA_Tipo_Documento));
                var categoriaItem = categoriasData.First(x => x.Id_SGA_Categoria_Documento.Equals(tipoCategoria.Id_SGA_Categoria_Documento));
                var condicionModel = condicionesEntities.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                var outputItem = new Documento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGACategoriaTipo = new CategoriaTipo
                    {
                        IdSGACategoriaTipo = tipoCategoria.Id_SGA_Categoria_Tipo,
                        TipoDocumento = new TipoDocumento
                        {
                            IdSGATipoDocumento = tipoItem.Id_SGA_Tipo_Documento,
                            Descripcion = tipoItem.Descripcion,
                            Codigo = tipoItem.Codigo,
                            Formato = tipoItem.Formato,
                            Estado = tipoItem.Estado
                        },
                        CategoriaDocumento = new CategoriaDocumento
                        {
                            Id_SGA_Categoria_Documento = categoriaItem.Id_SGA_Categoria_Documento,
                            Descripcion = categoriaItem.Descripcion,
                            Estado = categoriaItem.Estado
                        },
                        Estado = tipoCategoria.Estado
                    },
                    DocCodigo = item.Doc_Codigo,
                    DocNombre = item.Doc_Nombre,
                    Principal = item.Principal,
                    DocVersion = item.Doc_Version,
                    FechaAprobacion = item.Fecha_Aprobacion,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = condicionModel.Id_SGA_Condicion,
                        Descripcion = condicionModel.Descripcion,
                        Estado = condicionModel.Estado
                    },
                    Comentario = item.Comentario,
                    FechaTentativa = item.Fecha_Tentativa,
                    PdfRuta = item.Pdf_Ruta,
                    EditableRuta = item.Editable_Ruta,
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
                if (item.Doc_Predecesor.HasValue)
                {
                    outputItem.DocPredecesor = new Documento
                    {
                        IdSGADocumento = item.Doc_Predecesor.Value
                    };
                }
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Documento>> GetDocumentos()
        {
            var output = new List<Documento>();

            var data = await _context.SGA_Documentos.ToListAsync();

            var tiposCategoria = await _context.SGA_Categoria_Tipo.ToListAsync();
            var categoriasData = await _context.SGA_Categoria_Documento.ToListAsync();
            var tiposData = await _context.SGA_Tipos_Documento.ToListAsync();

            var condiciones = await _context.SGA_Condiciones.ToListAsync();

            var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            foreach (var item in data)
            {
                var tipoCategoria = tiposCategoria.First(x => x.Id_SGA_Categoria_Tipo.Equals(item.Id_SGA_Categoria_Tipo));
                var tipoItem = tiposData.First(x => x.Id_SGA_Tipo_Documento.Equals(tipoCategoria.Id_SGA_Tipo_Documento));
                var categoriaItem = categoriasData.First(x => x.Id_SGA_Categoria_Documento.Equals(tipoCategoria.Id_SGA_Categoria_Documento));
                var condicionModel = condiciones.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                var outputItem = new Documento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGACategoriaTipo = new CategoriaTipo
                    {
                        IdSGACategoriaTipo = tipoCategoria.Id_SGA_Categoria_Tipo,
                        TipoDocumento = new TipoDocumento
                        {
                            IdSGATipoDocumento = tipoItem.Id_SGA_Tipo_Documento,
                            Descripcion = tipoItem.Descripcion,
                            Codigo = tipoItem.Codigo,
                            Formato = tipoItem.Formato,
                            Estado = tipoItem.Estado
                        },
                        CategoriaDocumento = new CategoriaDocumento
                        {
                            Id_SGA_Categoria_Documento = categoriaItem.Id_SGA_Categoria_Documento,
                            Descripcion = categoriaItem.Descripcion,
                            Estado = categoriaItem.Estado
                        },
                        Estado = tipoCategoria.Estado
                    },
                    DocCodigo = item.Doc_Codigo,
                    DocNombre = item.Doc_Nombre,
                    Principal = item.Principal,
                    DocVersion = item.Doc_Version,
                    FechaAprobacion = item.Fecha_Aprobacion,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = condicionModel.Id_SGA_Condicion,
                        Descripcion = condicionModel.Descripcion,
                        Estado = condicionModel.Estado
                    },
                    Comentario = item.Comentario,
                    FechaTentativa = item.Fecha_Tentativa,
                    PdfRuta = item.Pdf_Ruta,
                    EditableRuta = item.Editable_Ruta,
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
                if (item.Doc_Predecesor.HasValue)
                {
                    outputItem.DocPredecesor = new Documento
                    {
                        IdSGADocumento = item.Doc_Predecesor.Value
                    };
                }
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Documento>> GetPredecesores(int Id)
        {
            var output = new List<Documento>();

            var data = await _context.SGA_Documentos.ToListAsync();
            //Nueva lista con Ids y Ids de Predecesores
            var dataIdPred = data.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) || x.Estado.Equals(EstadosDocumentosSGA.Superado.ToString())).Select(x => new { Id = x.Id_SGA_Documento, IdPred = x.Doc_Predecesor }).ToList();
            //Definir el documento de Origen
            var origen = data.First(x => x.Id_SGA_Documento.Equals(Id));

            //Si el origen tiene predecesor iniciar busqueda
            if (origen.Doc_Predecesor.HasValue)
            {
                //Variable temporal que almacena el Id del predecesor
                int? Idtemp = origen.Doc_Predecesor;
                //Lista de Ids de documentos predecesores
                var idsDocsPredecesores = new List<int>();
                //Buscar hasta que no haya predecesores
                while (Idtemp != null)
                {
                    //Buscar el documento con Id igual al predecesor de documento actual
                    var nuevoPred = dataIdPred.Find(x => x.Id.Equals(Idtemp.Value));
                    //Agregar el Id del documento predecesor a la lista
                    idsDocsPredecesores.Add(nuevoPred.Id);
                    //Cambiar el valor de la variable temporal al Id del predecesor del documento encontrado
                    Idtemp = nuevoPred.IdPred;
                }

                var predecesores = new List<Entities.SGA_Documentos.Documento>();
                //Agregar los documentos a la lista de predecesores en funcion a la lista de Ids de predecesores
                predecesores.AddRange(data.Where(x => idsDocsPredecesores.Contains(x.Id_SGA_Documento)));

                var tiposCategoria = await _context.SGA_Categoria_Tipo.ToListAsync();
                var categoriasData = await _context.SGA_Categoria_Documento.ToListAsync();
                var tiposData = await _context.SGA_Tipos_Documento.ToListAsync();
                var condiciones = await _context.SGA_Condiciones.ToListAsync();
                var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

                foreach (var item in predecesores)
                {
                    var tipoCategoria = tiposCategoria.First(x => x.Id_SGA_Categoria_Tipo.Equals(item.Id_SGA_Categoria_Tipo));
                    var tipoItem = tiposData.First(x => x.Id_SGA_Tipo_Documento.Equals(tipoCategoria.Id_SGA_Tipo_Documento));
                    var categoriaItem = categoriasData.First(x => x.Id_SGA_Categoria_Documento.Equals(tipoCategoria.Id_SGA_Categoria_Documento));
                    var condicionModel = condiciones.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                    var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                    var outputItem = new Documento
                    {
                        IdSGADocumento = item.Id_SGA_Documento,
                        SGACategoriaTipo = new CategoriaTipo
                        {
                            IdSGACategoriaTipo = tipoCategoria.Id_SGA_Categoria_Tipo,
                            TipoDocumento = new TipoDocumento
                            {
                                IdSGATipoDocumento = tipoItem.Id_SGA_Tipo_Documento,
                                Descripcion = tipoItem.Descripcion,
                                Codigo = tipoItem.Codigo,
                                Formato = tipoItem.Formato,
                                Estado = tipoItem.Estado
                            },
                            CategoriaDocumento = new CategoriaDocumento
                            {
                                Id_SGA_Categoria_Documento = categoriaItem.Id_SGA_Categoria_Documento,
                                Descripcion = categoriaItem.Descripcion,
                                Estado = categoriaItem.Estado
                            },
                            Estado = tipoCategoria.Estado
                        },
                        DocCodigo = item.Doc_Codigo,
                        DocNombre = item.Doc_Nombre,
                        Principal = item.Principal,
                        DocVersion = item.Doc_Version,
                        FechaAprobacion = item.Fecha_Aprobacion,
                        Condicion = new Condicion
                        {
                            IdSGACondicion = condicionModel.Id_SGA_Condicion,
                            Descripcion = condicionModel.Descripcion,
                            Estado = condicionModel.Estado
                        },
                        Comentario = item.Comentario,
                        FechaTentativa = item.Fecha_Tentativa,
                        PdfRuta = item.Pdf_Ruta,
                        EditableRuta = item.Editable_Ruta,
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
            }
            return output;
        }
        public async Task<List<Documento>> GetSecundarios(int Id)
        {
            var output = new List<Documento>();

            var relacionSecundarios = await _context.SGA_Documentos_Relacionados.Where(x => x.Id_Documento_Principal.Equals(Id) && x.Estado.Equals("ACT")).Select(x => x.Id_Documento_Secundario).ToListAsync();

            var data = await _context.SGA_Documentos.Where(x => relacionSecundarios.Contains(x.Id_SGA_Documento)).ToListAsync();

            var tiposCategoria = await _context.SGA_Categoria_Tipo.ToListAsync();
            var categoriasData = await _context.SGA_Categoria_Documento.ToListAsync();
            var tiposData = await _context.SGA_Tipos_Documento.ToListAsync();
            var condiciones = await _context.SGA_Condiciones.ToListAsync();
            var usuarios = await _context.Usuarios.Where(x => data.Select(y => y.Id_Usuario).Contains(x.Id_Usuario)).ToListAsync();

            foreach (var item in data)
            {
                var tipoCategoria = tiposCategoria.First(x => x.Id_SGA_Categoria_Tipo.Equals(item.Id_SGA_Categoria_Tipo));
                var tipoItem = tiposData.First(x => x.Id_SGA_Tipo_Documento.Equals(tipoCategoria.Id_SGA_Tipo_Documento));
                var categoriaItem = categoriasData.First(x => x.Id_SGA_Categoria_Documento.Equals(tipoCategoria.Id_SGA_Categoria_Documento));
                var condicionModel = condiciones.First(x => x.Id_SGA_Condicion.Equals(item.Condicion));
                var usuario = usuarios.First(x => x.Id_Usuario.Equals(item.Id_Usuario));

                var outputItem = new Documento
                {
                    IdSGADocumento = item.Id_SGA_Documento,
                    SGACategoriaTipo = new CategoriaTipo
                    {
                        IdSGACategoriaTipo = tipoCategoria.Id_SGA_Categoria_Tipo,
                        TipoDocumento = new TipoDocumento
                        {
                            IdSGATipoDocumento = tipoItem.Id_SGA_Tipo_Documento,
                            Descripcion = tipoItem.Descripcion,
                            Codigo = tipoItem.Codigo,
                            Formato = tipoItem.Formato,
                            Estado = tipoItem.Estado
                        },
                        CategoriaDocumento = new CategoriaDocumento
                        {
                            Id_SGA_Categoria_Documento = categoriaItem.Id_SGA_Categoria_Documento,
                            Descripcion = categoriaItem.Descripcion,
                            Estado = categoriaItem.Estado
                        },
                        Estado = tipoCategoria.Estado
                    },
                    DocCodigo = item.Doc_Codigo,
                    DocNombre = item.Doc_Nombre,
                    Principal = item.Principal,
                    DocVersion = item.Doc_Version,
                    FechaAprobacion = item.Fecha_Aprobacion,
                    Condicion = new Condicion
                    {
                        IdSGACondicion = condicionModel.Id_SGA_Condicion,
                        Descripcion = condicionModel.Descripcion,
                        Estado = condicionModel.Estado
                    },
                    Comentario = item.Comentario,
                    FechaTentativa = item.Fecha_Tentativa,
                    PdfRuta = item.Pdf_Ruta,
                    EditableRuta = item.Editable_Ruta,
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
        //No se utiliza
        public async Task<RevisionDocumento> GetRevisionDocumento(int Id)
        {            
            var data = await _context.SGA_Revisiones_Documento.FirstAsync(x=>x.Id_SGA_Revision_Documento.Equals(Id));
            var usuario = await _context.Usuarios.FindAsync(data.Id_Usuario);
            var output = new RevisionDocumento { 
                IdSGARevisionDocumento = data.Id_SGA_Revision_Documento,
                IdSGADocumento = data.Id_SGA_Documento,
                Orden = data.Orden,
                FechaLimite = data.Fecha_Limite,
                Comentario = data.Comentario,
                Estado = data.Estado,
                FechaRevision = data.Fecha_Revision,
                SGATipoRevision = new TipoRevision { IdTipoRevision = data.Id_SGA_Tipo_Revision},
                Usuario = new Domain.ValueObjects.Usuario {
                    IdUsuario = usuario.Id_Usuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Email = usuario.Email,
                    IdRol = usuario.Id_Rol,
                    UsuarioRed = usuario.usuario_red,
                    Estado = usuario.Estado                    
                }
            };
            return output;
        }
        public async Task<List<RevisionDocumento>> GetRevisionesDocumento(int IdDocumento)
        {
            var data = await _context.SGA_Revisiones_Documento.Where(x => x.Id_SGA_Documento.Equals(IdDocumento)).ToListAsync();
            var output = new List<RevisionDocumento>();
            foreach(var item in data)
            {
                var outputItem = new RevisionDocumento
                {
                    IdSGARevisionDocumento = item.Id_SGA_Revision_Documento,
                    IdSGADocumento = item.Id_SGA_Documento,
                    Orden = item.Orden,
                    FechaLimite = item.Fecha_Limite,
                    Comentario = item.Comentario,
                    Estado = item.Estado,
                    FechaRevision = item.Fecha_Revision,
                    SGATipoRevision = new TipoRevision { IdTipoRevision = item.Id_SGA_Tipo_Revision },
                    Usuario = new Domain.ValueObjects.Usuario { IdUsuario = item.Id_Usuario }
                };
                output.Add(outputItem);
            }
            
            return output;
        }
    }
}
