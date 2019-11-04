using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
   public class LevantarObservacionesRepository : ILevantarObservacionesReadOnlyRepository
    {
        private readonly Context _context;
        public LevantarObservacionesRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task LevantarObservacion(NuevaInspeccionDetalle nuevaInspeccionDetalle)
        {
            var detalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x=>x.Id_Inspeccion_Detalle.Equals(nuevaInspeccionDetalle.Id_Inspeccion_Detalle));
            detalle.Evidencia_Inspeccion = nuevaInspeccionDetalle.Evidencia_Inspeccion;
            detalle.Comentario_Inspeccion = nuevaInspeccionDetalle.Comentario_Inspeccion;
            detalle.Estado = "Implementado";
            detalle.Fecha_Cierre = DateTime.Now;
            _context.Inspecciones_Detalle.Update(detalle);
            await _context.SaveChangesAsync();
        }
    }
}
