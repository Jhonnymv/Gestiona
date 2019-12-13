using GestionAmbiental.Application.Repositories.Notificaciones;
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
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var detalle = new InspeccionDetalle();

            detalle.IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.TipoObservacion = inspeccionDetalle.Tipo_Observacion;
            detalle.DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.TipoCondicion = tipoCondicion_Domain;
            detalle.DescripcionAccion = inspeccionDetalle.Descripcion_Accion;
            detalle.EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.NivelRiesgo = inspeccionDetalle.Nivel_Riesgo;
            detalle.ResponsableMarcobre = responsableMarcobre_Domain;
            detalle.ResponsableContratista = inspeccionDetalle.Responsable_Contratista;
            detalle.FechaProgramada = inspeccionDetalle.Fecha_Programada;
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleImplementacion(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);
            var detalle = new InspeccionDetalle();

            detalle.IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.TipoObservacion = inspeccionDetalle.Tipo_Observacion;
            detalle.DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.TipoCondicion = tipoCondicion_Domain;
            detalle.DescripcionAccion = inspeccionDetalle.Descripcion_Accion;
            detalle.EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.NivelRiesgo = inspeccionDetalle.Nivel_Riesgo;
            detalle.ResponsableMarcobre = responsableMA_Domain;
            detalle.ResponsableContratista = inspeccionDetalle.Responsable_Contratista;
            detalle.FechaProgramada = inspeccionDetalle.Fecha_Programada;
            detalle.ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion;
            detalle.Asesor = responsableMA_Domain;
            detalle.FechaCierre = inspeccionDetalle.Fecha_Cierre;
            detalle.EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion;
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleRechazado(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);
            var detalle = new InspeccionDetalle();

            detalle.IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.TipoObservacion = inspeccionDetalle.Tipo_Observacion;
            detalle.DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.TipoCondicion = tipoCondicion_Domain;
            detalle.DescripcionAccion = inspeccionDetalle.Descripcion_Accion;
            detalle.EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.NivelRiesgo = inspeccionDetalle.Nivel_Riesgo;
            detalle.ResponsableMarcobre = responsableMA_Domain;
            detalle.ResponsableContratista = inspeccionDetalle.Responsable_Contratista;
            detalle.FechaProgramada = inspeccionDetalle.Fecha_Programada;
            detalle.ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion;
            detalle.Asesor = responsableMA_Domain;
            detalle.FechaCierre = inspeccionDetalle.Fecha_Cierre;
            detalle.EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion;
            detalle.FechaVerificacion = inspeccionDetalle.Fecha_Verificacion;
            detalle.ComentarioVerificacion = inspeccionDetalle.Comentario_Verificacion;

            return detalle;
        }
    }
}
