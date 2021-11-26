using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Notificaciones
{
    public class GetInspeccionDetalleInfoRepository : IGetInspeccionDetalleInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionDetalleInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionDetalle> GetInspeccionDetalle(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion)&&x.Estado!=EstadosInspeccion.Eliminado.ToString());
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion();
            if (tipoCondicion != null)
            {
                tipoCondicion_Domain.IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion;
                tipoCondicion_Domain.Descripcion = tipoCondicion.Descripcion;
                tipoCondicion_Domain.Grupo = tipoCondicion.Grupo;
            }
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre2 = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre2));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var detalle = new InspeccionDetalle
            {
                IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle,
                TipoObservacion = inspeccionDetalle.Tipo_Observacion,
                DescripcionActoCondicion = inspeccionDetalle.Descripcion_Acto_Condicion,
                DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion,
                TipoCondicion = tipoCondicion != null ? tipoCondicion_Domain : null,
                DescripcionAccion = inspeccionDetalle.Descripcion_Accion,
                EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion,
                NivelRiesgo = inspeccionDetalle.Nivel_Riesgo,
                ResponsableMarcobre = responsableMarcobre_Domain,
                ResponsableContratista = inspeccionDetalle.Responsable_Contratista,
                FechaProgramada = inspeccionDetalle.Fecha_Programada
            };
            if (responsableMarcobre2 != null)
            {
                var responsableMarcobre2_Domain = new Usuario(responsableMarcobre2.Id_Usuario, responsableMarcobre2.Id_Rol, responsableMarcobre2.Nombres, responsableMarcobre2.Apellidos, responsableMarcobre2.Email);
                detalle.ResponsableMarcobre2 = responsableMarcobre2_Domain;
            }
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleImplementacion(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion();
            if (tipoCondicion != null)
            {
                tipoCondicion_Domain.IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion;
                tipoCondicion_Domain.Descripcion = tipoCondicion.Descripcion;
                tipoCondicion_Domain.Grupo = tipoCondicion.Grupo;
            }
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre2 = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre2));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var Asesor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var Asesor_Domain = new Usuario(Asesor.Id_Usuario, Asesor.Id_Rol, Asesor.Nombres, Asesor.Apellidos, Asesor.Email);
            var detalle = new InspeccionDetalle
            {
                IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle,
                TipoObservacion = inspeccionDetalle.Tipo_Observacion,
                DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion,
                DescripcionActoCondicion = inspeccionDetalle.Descripcion_Acto_Condicion,
                TipoCondicion = tipoCondicion != null ? tipoCondicion_Domain : null,
                DescripcionAccion = inspeccionDetalle.Descripcion_Accion,
                EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion,
                NivelRiesgo = inspeccionDetalle.Nivel_Riesgo,
                ResponsableMarcobre = responsableMarcobre_Domain,
                ResponsableContratista = inspeccionDetalle.Responsable_Contratista,
                FechaProgramada = inspeccionDetalle.Fecha_Programada,
                ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion,
                Asesor = Asesor_Domain,
                FechaCierre = inspeccionDetalle.Fecha_Cierre,
                EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion
            };

            if (responsableMarcobre2 != null)
            {
                var responsableMarcobre2_Domain = new Usuario(responsableMarcobre2.Id_Usuario, responsableMarcobre2.Id_Rol, responsableMarcobre2.Nombres, responsableMarcobre2.Apellidos, responsableMarcobre2.Email);
                detalle.ResponsableMarcobre2 = responsableMarcobre2_Domain;
            }
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleRechazado(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion();
            if (tipoCondicion != null)
            {
                tipoCondicion_Domain.IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion; 
                tipoCondicion_Domain.Descripcion = tipoCondicion.Descripcion;
                tipoCondicion_Domain.Grupo = tipoCondicion.Grupo;
            }
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre2 = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre2));            
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var Asesor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var Asesor_Domain = new Usuario(Asesor.Id_Usuario, Asesor.Id_Rol, Asesor.Nombres, Asesor.Apellidos, Asesor.Email);
            var detalle = new InspeccionDetalle
            {
                IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle,
                TipoObservacion = inspeccionDetalle.Tipo_Observacion,
                DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion,
                DescripcionActoCondicion = inspeccionDetalle.Descripcion_Acto_Condicion,
                TipoCondicion = tipoCondicion != null ? tipoCondicion_Domain : null,
                DescripcionAccion = inspeccionDetalle.Descripcion_Accion,
                EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion,
                NivelRiesgo = inspeccionDetalle.Nivel_Riesgo,
                ResponsableMarcobre = responsableMarcobre_Domain,
                ResponsableContratista = inspeccionDetalle.Responsable_Contratista,
                FechaProgramada = inspeccionDetalle.Fecha_Programada,
                ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion,
                Asesor = Asesor_Domain,
                FechaCierre = inspeccionDetalle.Fecha_Cierre,
                EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion,
                FechaVerificacion = inspeccionDetalle.Fecha_Verificacion,
                ComentarioVerificacion = inspeccionDetalle.Comentario_Verificacion
            };

            if (responsableMarcobre2 != null)
            {
                var responsableMarcobre2_Domain = new Usuario(responsableMarcobre2.Id_Usuario, responsableMarcobre2.Id_Rol, responsableMarcobre2.Nombres, responsableMarcobre2.Apellidos, responsableMarcobre2.Email);
                detalle.ResponsableMarcobre2 = responsableMarcobre2_Domain;
            }

            return detalle;
        }
    }
}
