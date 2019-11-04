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
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                detalles.Add(new InspeccionDetalle(
                    item.Id_Inspeccion_Cabecera,
                    item.Id_Inspeccion_Detalle,
                    item.Descripcion_Condicion,
                    new Domain.ValueObjects.Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion),
                    item.Evidencia_Condicion,
                    item.Nivel_Riesgo_Ambiental,
                    item.Descripcion_Accion,
                    new Domain.ValueObjects.Usuario(responsableMarc.Id_Usuario, responsableMarc.Id_Rol, responsableMarc.Nombres, responsableMarc.Apellidos, responsableMarc.Email),
                    item.Responsable_Contratista,
                    item.Fecha_Programada));
            }
            
            return detalles;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalle(int idDetalle)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(idDetalle));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion);
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario,responsableMarcobre.Id_Rol,responsableMarcobre.Nombres,responsableMarcobre.Apellidos,responsableMarcobre.Email);
            var detalle = new InspeccionDetalle(
                inspeccionDetalle.Id_Inspeccion_Cabecera,
                inspeccionDetalle.Id_Inspeccion_Detalle,
                inspeccionDetalle.Descripcion_Condicion,
                tipoCondicion_Domain,
                inspeccionDetalle.Evidencia_Condicion,
                inspeccionDetalle.Nivel_Riesgo_Ambiental,
                inspeccionDetalle.Descripcion_Accion,
                responsableMarcobre_Domain,
                inspeccionDetalle.Responsable_Contratista,
                inspeccionDetalle.Fecha_Programada
                );
            return detalle;
        }
    }
}
