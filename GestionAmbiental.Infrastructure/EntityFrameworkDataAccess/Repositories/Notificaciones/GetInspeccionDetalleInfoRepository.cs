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
            var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var detalle = new InspeccionDetalle();

            detalle.Id_Inspeccion_Cabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.Id_Inspeccion_Detalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.Tipo_Observacion = inspeccionDetalle.Tipo_Observacion;
            detalle.Descripcion_Condicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.Tipo_Condicion = tipoCondicion_Domain;
            detalle.Descripcion_Accion = inspeccionDetalle.Descripcion_Accion;
            detalle.Evidencia_Condicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.Nivel_Riesgo_Ambiental = inspeccionDetalle.Nivel_Riesgo_Ambiental;
            detalle.Responsable_Marcobre = responsableMarcobre_Domain;
            detalle.Responsable_Contratista = inspeccionDetalle.Responsable_Contratista;
            detalle.Fecha_Programada = inspeccionDetalle.Fecha_Programada;
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleImplementacion(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_MA));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);
            var detalle = new InspeccionDetalle();

            detalle.Id_Inspeccion_Cabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.Id_Inspeccion_Detalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.Tipo_Observacion = inspeccionDetalle.Tipo_Observacion;
            detalle.Descripcion_Condicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.Tipo_Condicion = tipoCondicion_Domain;
            detalle.Descripcion_Accion = inspeccionDetalle.Descripcion_Accion;
            detalle.Evidencia_Condicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.Nivel_Riesgo_Ambiental = inspeccionDetalle.Nivel_Riesgo_Ambiental;
            detalle.Responsable_Marcobre = responsableMA_Domain;
            detalle.Responsable_Contratista = inspeccionDetalle.Responsable_Contratista;
            detalle.Fecha_Programada = inspeccionDetalle.Fecha_Programada;
            detalle.Comentario_Inspeccion = inspeccionDetalle.Comentario_Inspeccion;
            detalle.Responsable_MA = responsableMA_Domain;
            detalle.Fecha_Cierre = inspeccionDetalle.Fecha_Cierre;
            detalle.Evidencia_Inspeccion = inspeccionDetalle.Evidencia_Inspeccion;
            return detalle;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalleRechazado(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_MA));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);
            var detalle = new InspeccionDetalle();

            detalle.Id_Inspeccion_Cabecera = inspeccionDetalle.Id_Inspeccion_Cabecera;
            detalle.Id_Inspeccion_Detalle = inspeccionDetalle.Id_Inspeccion_Detalle;
            detalle.Tipo_Observacion = inspeccionDetalle.Tipo_Observacion;
            detalle.Descripcion_Condicion = inspeccionDetalle.Descripcion_Condicion;
            detalle.Tipo_Condicion = tipoCondicion_Domain;
            detalle.Descripcion_Accion = inspeccionDetalle.Descripcion_Accion;
            detalle.Evidencia_Condicion = inspeccionDetalle.Evidencia_Condicion;
            detalle.Nivel_Riesgo_Ambiental = inspeccionDetalle.Nivel_Riesgo_Ambiental;
            detalle.Responsable_Marcobre = responsableMA_Domain;
            detalle.Responsable_Contratista = inspeccionDetalle.Responsable_Contratista;
            detalle.Fecha_Programada = inspeccionDetalle.Fecha_Programada;
            detalle.Comentario_Inspeccion = inspeccionDetalle.Comentario_Inspeccion;
            detalle.Responsable_MA = responsableMA_Domain;
            detalle.Fecha_Cierre = inspeccionDetalle.Fecha_Cierre;
            detalle.Evidencia_Inspeccion = inspeccionDetalle.Evidencia_Inspeccion;
            detalle.Fecha_Verificacion_MA = inspeccionDetalle.Fecha_Verificacion_MA;
            detalle.Comentario_MA = inspeccionDetalle.Comentario_MA;

            return detalle;
        }
    }
}
