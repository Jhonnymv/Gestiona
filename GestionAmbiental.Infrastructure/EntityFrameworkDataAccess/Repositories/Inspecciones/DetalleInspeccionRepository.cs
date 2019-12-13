using GestionAmbiental.Application.Repositories.Inspecciones;
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
            
            var inspeccionesDetalle = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            
            var usuarios = await _context.Usuarios.ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            foreach (var item in inspeccionesDetalle)
            {
                var responsableMarc = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                detalles.Add(new InspeccionDetalle(
                    item.Id_Inspeccion_Cabecera,
                    item.Id_Inspeccion_Detalle,
                    item.Tipo_Observacion,
                    item.Descripcion_Condicion,
                    new Domain.ValueObjects.TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion,tipoCondicion.Grupo),
                    item.Evidencia_Condicion,
                    item.Nivel_Riesgo,
                    item.Descripcion_Accion,
                    new Domain.ValueObjects.Usuario(responsableMarc.Id_Usuario, responsableMarc.Id_Rol, responsableMarc.Nombres, responsableMarc.Apellidos, responsableMarc.Email),
                    item.Responsable_Contratista,
                    item.Fecha_Programada,
                    item.Evidencia_Implementacion,
                    item.Comentario_Implementacion,
                    item.Fecha_Cierre,
                    new Domain.ValueObjects.Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres,responsableMA.Apellidos, responsableMA.Email),
                    item.Fecha_Verificacion,
                    item.Comentario_Verificacion,
                    item.Estado));
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

            var detalle = new InspeccionDetalle(
                inspeccionDetalle.Id_Inspeccion_Cabecera,
                inspeccionDetalle.Id_Inspeccion_Detalle,
                inspeccionDetalle.Tipo_Observacion,
                inspeccionDetalle.Descripcion_Condicion,
                tipoCondicion_Domain,
                inspeccionDetalle.Evidencia_Condicion,
                inspeccionDetalle.Nivel_Riesgo,
                inspeccionDetalle.Descripcion_Accion,
                responsableMarcobre_Domain,
                inspeccionDetalle.Responsable_Contratista,
                inspeccionDetalle.Fecha_Programada,
                inspeccionDetalle.Evidencia_Implementacion,
                inspeccionDetalle.Comentario_Implementacion,
                inspeccionDetalle.Fecha_Cierre,
                responsableMA_Domain,
                inspeccionDetalle.Fecha_Verificacion,
                inspeccionDetalle.Comentario_Verificacion,
                inspeccionDetalle.Estado
                );
            return detalle;
        }
    }
}
