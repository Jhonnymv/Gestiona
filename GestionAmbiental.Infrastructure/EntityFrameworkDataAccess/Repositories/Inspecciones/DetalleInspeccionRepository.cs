using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class DetalleInspeccionRepository : IDetalleInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public DetalleInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<InspeccionDetalle>> GetInspeccionDetalles(int idCabecera)
        {
            var detalles = new List<InspeccionDetalle>();
            
            var inspeccionesDetalle = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            
            var usuarios = await _context.Usuarios.ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            foreach (var item in inspeccionesDetalle)
            {
                var responsableMarc = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                detalles.Add(new InspeccionDetalle {
                    IdInspeccionCabecera = item.Id_Inspeccion_Cabecera,
                    IdInspeccionDetalle = item.Id_Inspeccion_Detalle,
                    TipoObservacion = item.Tipo_Observacion,
                    DescripcionCondicion = item.Descripcion_Condicion,
                    TipoCondicion = new Domain.ValueObjects.TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion,tipoCondicion.Grupo),
                    EvidenciaCondicion = item.Evidencia_Condicion,
                    NivelRiesgo = item.Nivel_Riesgo,
                    DescripcionAccion = item.Descripcion_Accion,
                    ResponsableMarcobre = new Domain.ValueObjects.Usuario(responsableMarc.Id_Usuario, responsableMarc.Id_Rol, responsableMarc.Nombres, responsableMarc.Apellidos, responsableMarc.Email),
                    ResponsableContratista = item.Responsable_Contratista,
                    CorreoContratista = item.Correo_Contratista,
                    FechaProgramada = item.Fecha_Programada,
                    EvidenciaImplementacion = item.Evidencia_Implementacion,
                    ComentarioImplementacion = item.Comentario_Implementacion,
                    FechaCierre = item.Fecha_Cierre,
                    Asesor = new Domain.ValueObjects.Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres,responsableMA.Apellidos, responsableMA.Email),
                    FechaVerificacion = item.Fecha_Verificacion,
                    ComentarioVerificacion = item.Comentario_Verificacion,
                    Estado = item.Estado,
                    DescripcionActoCondicion = item.Descripcion_Acto_Condicion
                });
            }
            
            return detalles;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalle(int idDetalle)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(idDetalle));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion,tipoCondicion.Grupo);
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario,responsableMarcobre.Id_Rol,responsableMarcobre.Nombres,responsableMarcobre.Apellidos,responsableMarcobre.Email);
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

            var detalle = new InspeccionDetalle {
                IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle,
                TipoObservacion = inspeccionDetalle.Tipo_Observacion,
                DescripcionActoCondicion = inspeccionDetalle.Descripcion_Acto_Condicion,
                DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion,
                TipoCondicion = tipoCondicion_Domain,
                EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion,
                NivelRiesgo = inspeccionDetalle.Nivel_Riesgo,
                DescripcionAccion = inspeccionDetalle.Descripcion_Accion,
                ResponsableMarcobre = responsableMarcobre_Domain,
                ResponsableContratista = inspeccionDetalle.Responsable_Contratista,
                CorreoContratista = inspeccionDetalle.Correo_Contratista,
                FechaProgramada = inspeccionDetalle.Fecha_Programada,
                EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion,
                ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion,
                FechaCierre = inspeccionDetalle.Fecha_Cierre,
                Asesor = responsableMA_Domain,
                FechaVerificacion = inspeccionDetalle.Fecha_Verificacion,
                ComentarioVerificacion = inspeccionDetalle.Comentario_Verificacion,
                Estado = inspeccionDetalle.Estado
                };
            return detalle;
        }
    }
}
