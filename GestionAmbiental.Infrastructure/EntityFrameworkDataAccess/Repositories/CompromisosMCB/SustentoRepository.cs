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
            var entity = new Entities.CompromisosMCB.SustentoCMCB {                
                Id_Compromiso = sustentoCMCB.CompromisoCMCB.Id,
                Fecha_limite = sustentoCMCB.Fecha_limite,
                Id_Responsable1 = sustentoCMCB.Responsable1.IdUsuario,
                Id_Responsable2 = sustentoCMCB.Responsable2.IdUsuario,
                Id_Asesor = sustentoCMCB.Asesor.IdUsuario,
                Detalle_compromiso = sustentoCMCB.Detalle_compromiso,
                Medio_verificacion = sustentoCMCB.Medio_verificacion,
                Estado = sustentoCMCB.Estado,
                Comentario_implementacion = sustentoCMCB.Comentario_implementacion,
                Comentario_verificacion = sustentoCMCB.Comentario_verificacion,
                Evidencia = sustentoCMCB.Evidencia,
                Sustento_cumplimiento = sustentoCMCB.Sustento_cumplimiento,
                Fecha_validacion = sustentoCMCB.Fecha_validacion,
                Fecha_entrega = sustentoCMCB.Fecha_entrega                
            };

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

        public async Task<List<SustentoCMCB>> ListarSustentos(int[] gerencias, int[] areas, int[] criterios, string[] estados)
        {
            var sustentos = await _context.SustentoCMCB.ToListAsync();
            var compromisos = await _context.CompromisoCMCB.ToListAsync();

            if (gerencias.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Gerencia.HasValue && gerencias.Contains(x.Id_Gerencia.Value)).ToList();
            }
            if (areas.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Area.HasValue && areas.Contains(x.Id_Area.Value)).ToList();
            }
            if (criterios.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Criterio.HasValue && criterios.Contains(x.Id_Criterio.Value)).ToList();
            }
            if (gerencias.Length > 0|| areas.Length > 0|| criterios.Length > 0)
            {
                sustentos = sustentos.Where(x => compromisos.Select(y => y.Id).Contains(x.Id_Compromiso.Value)).ToList();
            }
            estados = estados.Where(x => x != null).ToArray();
            if (estados.Length > 0)
            {
                sustentos = sustentos.Where(x => estados.Contains(x.Estado)).ToList();
            }

            var output = new List<SustentoCMCB>();
            foreach(var sustento in sustentos)
            {

                var compromiso = await _context.CompromisoCMCB.FindAsync(sustento.Id_Compromiso);
                var compromisoDomain = new CompromisoCMCB
                {
                    Id = compromiso.Id,
                    Detalle_compromiso = compromiso.Detalle_compromiso,
                };
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
                    outputItem.Responsable2 = responsable2Domain;
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
    }
}
