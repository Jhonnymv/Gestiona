using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class SustentoRepository: ISustentoReadOnlyRepository
    {
        private readonly Context _context;

        public SustentoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> RegistrarSustento(SustentoCMCB sustentoCMCB)
        {
            var entity = new Entities.CompromisosMCB.SustentoCMCB {
                Id_Compromiso = sustentoCMCB.CompromisoCMCB.Id,
                Fecha_limite = sustentoCMCB.Fecha_limite,
                Id_Responsable1 = sustentoCMCB.Responsable1.IdUsuario,
                Id_Responsable2 = sustentoCMCB.Responsable2.IdUsuario,
                Detalle_compromiso = sustentoCMCB.Detalle_compromiso,
                Medio_verificacion = sustentoCMCB.Medio_verificacion,
                Estado = sustentoCMCB.Estado
            };

            await _context.SustentoCMCB.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task EditarSustento(SustentoCMCB sustentoCMCB)
        {
            var entity = await _context.SustentoCMCB.FindAsync(sustentoCMCB.Id);

            entity.Id_Compromiso = sustentoCMCB.CompromisoCMCB != null ? sustentoCMCB.CompromisoCMCB.Id : entity.Id_Compromiso;
            entity.Fecha_limite = sustentoCMCB.Fecha_limite != null ? sustentoCMCB.Fecha_limite:entity.Fecha_limite;
            entity.Id_Responsable1 = sustentoCMCB.Responsable1 != null ? sustentoCMCB.Responsable1.IdUsuario : entity.Id_Responsable1;
            entity.Id_Responsable2 = sustentoCMCB.Responsable2 != null ? sustentoCMCB.Responsable2.IdUsuario : entity.Id_Responsable2;
            entity.Id_Asesor = sustentoCMCB.Asesor != null ? sustentoCMCB.Asesor.IdUsuario : entity.Id_Asesor;
            entity.Detalle_compromiso = sustentoCMCB.Detalle_compromiso != null ? sustentoCMCB.Detalle_compromiso: entity.Detalle_compromiso;
            entity.Medio_verificacion = sustentoCMCB.Medio_verificacion != null ? sustentoCMCB.Medio_verificacion : entity.Medio_verificacion;
            entity.Estado = sustentoCMCB.Estado != null ? sustentoCMCB.Estado : entity.Estado;
            entity.Comentario_implementacion = sustentoCMCB.Comentario_implementacion != null ? sustentoCMCB.Comentario_implementacion : entity.Comentario_implementacion;
            entity.Comentario_verificacion = sustentoCMCB.Comentario_verificacion != null ? sustentoCMCB.Comentario_verificacion : entity.Comentario_verificacion;
            entity.Evidencia = sustentoCMCB.Evidencia != null ? sustentoCMCB.Evidencia : entity.Evidencia;
            entity.Sustento_cumplimiento = sustentoCMCB.Sustento_cumplimiento != null ? sustentoCMCB.Sustento_cumplimiento : entity.Sustento_cumplimiento;
            entity.Fecha_validacion = sustentoCMCB.Fecha_validacion != null ? sustentoCMCB.Fecha_validacion : entity.Fecha_validacion;
            entity.Fecha_entrega = sustentoCMCB.Fecha_entrega != null ? sustentoCMCB.Fecha_entrega : entity.Fecha_entrega;


            _context.SustentoCMCB.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<SustentoCMCB> ObtenerSustento(int Id)
        {
            var sustento = await _context.SustentoCMCB.FindAsync(Id);

            var compromiso = await _context.CompromisoCMCB.FindAsync(sustento.Id_Compromiso);
            var compromisoDomain = new CompromisoCMCB{
                Id = compromiso.Id,
                Detalle_compromiso= compromiso.Detalle_compromiso                
            };
            var usuarios = await _context.Usuarios.ToListAsync();
            var responsable1 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable1));
            var responsable2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable2));
            var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Asesor));

            var output = new SustentoCMCB
            {
                Id = sustento.Id,
                CompromisoCMCB = compromisoDomain,
                Fecha_limite = sustento.Fecha_limite,
                Fecha_entrega = sustento.Fecha_entrega,
                Fecha_validacion = sustento.Fecha_validacion,
                Estado = sustento.Estado,
                Sustento_cumplimiento = sustento.Sustento_cumplimiento,
                Detalle_compromiso = sustento.Detalle_compromiso,
                Evidencia = sustento.Evidencia,
                Comentario_implementacion = sustento.Comentario_implementacion,
                Comentario_verificacion = sustento.Comentario_verificacion,
                Medio_verificacion = sustento.Medio_verificacion                
            };

            if (responsable1 != null)
            {
                var responsable1Domain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = responsable1.Id_Usuario,
                    Nombres = responsable1.Nombres,
                    Apellidos = responsable1.Apellidos,
                    DNI = responsable1.DNI,
                    UsuarioRed = responsable1.usuario_red,
                    Puesto = responsable1.Puesto,
                    Celular = responsable1.Celular,
                    Email = responsable1.Email,
                    IdRol = responsable1.Id_Rol,
                    Estado = responsable1.Estado
                };
                output.Responsable1 = responsable1Domain;
            }
            if (responsable2 != null)
            {
                var responsable2Domain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = responsable1.Id_Usuario,
                    Nombres = responsable1.Nombres,
                    Apellidos = responsable1.Apellidos,
                    DNI = responsable1.DNI,
                    UsuarioRed = responsable1.usuario_red,
                    Puesto = responsable1.Puesto,
                    Celular = responsable1.Celular,
                    Email = responsable1.Email,
                    IdRol = responsable1.Id_Rol,
                    Estado = responsable1.Estado
                };
                output.Responsable2 = responsable2Domain;
            }
            if (asesor != null)
            {
                var asesorDomain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = responsable1.Id_Usuario,
                    Nombres = responsable1.Nombres,
                    Apellidos = responsable1.Apellidos,
                    DNI = responsable1.DNI,
                    UsuarioRed = responsable1.usuario_red,
                    Puesto = responsable1.Puesto,
                    Celular = responsable1.Celular,
                    Email = responsable1.Email,
                    IdRol = responsable1.Id_Rol,
                    Estado = responsable1.Estado
                };
                output.Asesor = asesorDomain;
            }
            return output;
        }
        public async Task<List<Domain.CompromisosMCB.CompromisoCMCB>> ListarCompromisos()
        {
            //Consultar registros
            var filtrados = await _context.CompromisoCMCB.ToListAsync();

            //Nueva lista en blanco
            var output = new List<Domain.CompromisosMCB.CompromisoCMCB>();

            var criterios = await _context.CriterioCMCB.ToListAsync();
            //Mapear registros en nueva lista
            foreach (var item in filtrados)
            {
                var dataCriterio = await _context.CriterioCMCB.FindAsync(item.Id_Criterio);
                var dataEtapa = await _context.EtapaCMCB.FindAsync(item.Id_Etapa);
                var dataOrigen = await _context.OrigenCMCB.FindAsync(item.Id_Origen);
                var dataAutoridad = await _context.AutoridadCMCB.FindAsync(item.Id_Autoridad);

                var dataAspecto = await _context.AspectoCMCB.FindAsync(item.Id_Aspecto);
                var dataComponente = await _context.ComponenteCMCB.FindAsync(item.Id_Componente);
                var dataGerencia = await _context.GerenciaCMCB.FindAsync(item.Id_Gerencia);
                var dataArea = await _context.AreaCMCB.FindAsync(item.Id_Area);
                var dataResp1 = await _context.Usuarios.FindAsync(item.Id_Responsable1);
                var dataResp2 = await _context.Usuarios.FindAsync(item.Id_Responsable2);
                var dataAprueba = await _context.Usuarios.FindAsync(item.Id_Aprueba);


                output.Add(new Domain.CompromisosMCB.CompromisoCMCB
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    CriterioCMCB = item.Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)item.Id_Criterio, Criterio = dataCriterio.Criterio, Detalle = dataCriterio.Detalle } : null,
                    Detalle_compromiso = item.Detalle_compromiso,
                    EtapaCMCB = item.Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)item.Id_Etapa, Etapa = dataEtapa.Etapa } : null,
                    Texto_compromiso = item.Texto_compromiso,
                    OrigenCMCB = item.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)item.Id_Origen, Origen = dataOrigen.Origen } : null,
                    Tomo = item.Tomo,
                    Capitulo = item.Capitulo,
                    Seccion = item.Seccion,
                    Pagina = item.Pagina,
                    Resolucion = item.Resolucion,
                    AutoridadCMCB = item.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)item.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                    Fecha = item.Fecha,

                    AspectoCMCB = item.Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)item.Id_Aspecto, Aspecto = dataAspecto.Aspecto } : null,
                    ComponenteCMCB = item.Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)item.Id_Componente, Componente = dataComponente.Componente } : null,
                    GerenciaCMCB = item.Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)item.Id_Gerencia, Gerencia = dataGerencia.Gerencia, Descripcion = dataGerencia.Descripcion } : null,
                    AreaCMCB = item.Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)item.Id_Area, Area = dataArea.Area } : null,
                    Frecuencia = item.Frecuencia,
                    Responsable1 = item.Id_Responsable1 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable1, Nombres = dataResp1.Nombres, DNI = dataResp1.DNI } : null,
                    Responsable2 = item.Id_Responsable2 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable2, Nombres = dataResp2.Nombres, DNI = dataResp2.DNI } : null,
                    Frecuencia_reporte = item.Frecuencia_reporte,
                    Sustentos = item.Sustentos,
                    Estado = item.Estado,
                    Aprueba = item.Id_Aprueba != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable2, Nombres = dataAprueba.Nombres, DNI = dataAprueba.DNI } : null,
                    Medio_verificacion = item.Medio_verificacion
                });
            }
            //retornar nueva lista
            return output;
        }
        public async Task<List<SustentoCMCB>> ListarSustentos(int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            var sustentos = await _context.SustentoCMCB.ToListAsync();
            var compromisos = await ListarCompromisos();

            if (gerencias.Length > 0)
            {
                compromisos = compromisos.Where(x => x.GerenciaCMCB!=null && gerencias.Contains(x.GerenciaCMCB.Id)).ToList();
            }
            if (areas.Length > 0)
            {
                compromisos = compromisos.Where(x => x.AreaCMCB != null && areas.Contains(x.AreaCMCB.Id)).ToList();
            }
            if (criterios.Length > 0)
            {
                compromisos = compromisos.Where(x => x.CriterioCMCB != null && criterios.Contains(x.CriterioCMCB.Id)).ToList();
            }
            if (etapas.Length > 0)
            {
                compromisos = compromisos.Where(x => x.EtapaCMCB != null && etapas.Contains(x.EtapaCMCB.Id)).ToList();
            }
            if (gerencias.Length > 0|| areas.Length > 0|| criterios.Length > 0 || etapas.Length > 0)
            {
                sustentos = sustentos.Where(x => compromisos.Select(y => y.Id).Contains(x.Id_Compromiso.Value)).ToList();
            }
            if (estados.Length > 0)
            {
                sustentos = sustentos.Where(x => estados.Contains(x.Estado)).ToList();
            }

            var output = new List<SustentoCMCB>();
            foreach(var sustento in sustentos)
            {

                var compromisoDomain = compromisos.FirstOrDefault(x=>x.Id.Equals(sustento.Id_Compromiso));                
                var usuarios = await _context.Usuarios.ToListAsync();
                var responsable1 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable1));
                var responsable2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable2));
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Asesor));

                var outputItem = new SustentoCMCB
                {
                    Id = sustento.Id,
                    CompromisoCMCB = compromisoDomain,
                    Fecha_limite = sustento.Fecha_limite,
                    Fecha_entrega = sustento.Fecha_entrega,
                    Fecha_validacion = sustento.Fecha_validacion,
                    Estado = sustento.Estado,
                    Sustento_cumplimiento = sustento.Sustento_cumplimiento,
                    Detalle_compromiso = sustento.Detalle_compromiso,
                    Evidencia = sustento.Evidencia,
                    Comentario_implementacion = sustento.Comentario_implementacion,
                    Comentario_verificacion = sustento.Comentario_verificacion,
                    Medio_verificacion = sustento.Medio_verificacion
                };

                if (responsable1 != null)
                {
                    var responsable1Domain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres,
                        Apellidos = responsable1.Apellidos,
                        DNI = responsable1.DNI,
                        UsuarioRed = responsable1.usuario_red,
                        Puesto = responsable1.Puesto,
                        Celular = responsable1.Celular,
                        Email = responsable1.Email,
                        IdRol = responsable1.Id_Rol,
                        Estado = responsable1.Estado
                    };
                    outputItem.Responsable1 = responsable1Domain;
                }
                if (responsable2 != null)
                {
                    var responsable2Domain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable2.Id_Usuario,
                        Nombres = responsable2.Nombres,
                        Apellidos = responsable2.Apellidos,
                        DNI = responsable2.DNI,
                        UsuarioRed = responsable2.usuario_red,
                        Puesto = responsable2.Puesto,
                        Celular = responsable2.Celular,
                        Email = responsable2.Email,
                        IdRol = responsable2.Id_Rol,
                        Estado = responsable2.Estado
                    };
                    outputItem.Responsable2 = responsable2Domain;
                }
                if (asesor != null)
                {
                    var asesorDomain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres,
                        Apellidos = asesor.Apellidos,
                        DNI = asesor.DNI,
                        UsuarioRed = asesor.usuario_red,
                        Puesto = asesor.Puesto,
                        Celular = asesor.Celular,
                        Email = asesor.Email,
                        IdRol = asesor.Id_Rol,
                        Estado = asesor.Estado
                    };
                    outputItem.Asesor = asesorDomain;
                }
                output.Add(outputItem);
            }
            return output;
        }

        public async Task EliminarSustento(int Id)
        {
            var entity = await _context.SustentoCMCB.FindAsync(Id);
            entity.Estado = EstadosSustento.Eliminado.ToString();
            _context.SustentoCMCB.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<SustentoCMCB>> ListarSustentosByCompromiso(int IdCompromiso)
        {
            var sustentos = await _context.SustentoCMCB.ToListAsync();
            var compromisos = await ListarCompromisos();

            sustentos = sustentos.Where(x => x.Id_Compromiso.Equals(IdCompromiso)).ToList();


            var output = new List<SustentoCMCB>();
            foreach (var sustento in sustentos)
            {

                var compromisoDomain = compromisos.FirstOrDefault(x => x.Id.Equals(IdCompromiso));
                var usuarios = await _context.Usuarios.ToListAsync();
                var responsable1 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable1));
                var responsable2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Responsable2));
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(sustento.Id_Asesor));

                var outputItem = new SustentoCMCB
                {
                    Id = sustento.Id,
                    CompromisoCMCB = compromisoDomain,
                    Fecha_limite = sustento.Fecha_limite,
                    Fecha_entrega = sustento.Fecha_entrega,
                    Fecha_validacion = sustento.Fecha_validacion,
                    Estado = sustento.Estado,
                    Sustento_cumplimiento = sustento.Sustento_cumplimiento,
                    Detalle_compromiso = sustento.Detalle_compromiso,
                    Evidencia = sustento.Evidencia,
                    Comentario_implementacion = sustento.Comentario_implementacion,
                    Comentario_verificacion = sustento.Comentario_verificacion,
                    Medio_verificacion = sustento.Medio_verificacion
                };

                if (responsable1 != null)
                {
                    var responsable1Domain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres,
                        Apellidos = responsable1.Apellidos,
                        DNI = responsable1.DNI,
                        UsuarioRed = responsable1.usuario_red,
                        Puesto = responsable1.Puesto,
                        Celular = responsable1.Celular,
                        Email = responsable1.Email,
                        IdRol = responsable1.Id_Rol,
                        Estado = responsable1.Estado
                    };
                    outputItem.Responsable1 = responsable1Domain;
                }
                if (responsable2 != null)
                {
                    var responsable2Domain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable2.Id_Usuario,
                        Nombres = responsable2.Nombres,
                        Apellidos = responsable2.Apellidos,
                        DNI = responsable2.DNI,
                        UsuarioRed = responsable2.usuario_red,
                        Puesto = responsable2.Puesto,
                        Celular = responsable2.Celular,
                        Email = responsable2.Email,
                        IdRol = responsable2.Id_Rol,
                        Estado = responsable2.Estado
                    };
                    outputItem.Responsable2 = responsable2Domain;
                }
                if (asesor != null)
                {
                    var asesorDomain = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres,
                        Apellidos = asesor.Apellidos,
                        DNI = asesor.DNI,
                        UsuarioRed = asesor.usuario_red,
                        Puesto = asesor.Puesto,
                        Celular = asesor.Celular,
                        Email = asesor.Email,
                        IdRol = asesor.Id_Rol,
                        Estado = asesor.Estado
                    };
                    outputItem.Asesor = asesorDomain;
                }
                output.Add(outputItem);
            }
            return output;
        }
    }
}
