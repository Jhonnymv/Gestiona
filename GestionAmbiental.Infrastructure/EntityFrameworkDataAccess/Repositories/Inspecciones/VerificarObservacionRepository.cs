using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class VerificarObservacionRepository: IVerificarObservacionReadOnlyRepository
    {
        private readonly Context _context;
        public VerificarObservacionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task VerificarObservacionMA(NuevaInspeccionDetalle nuevaInspeccionDetalle)
        {
            var detalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(nuevaInspeccionDetalle.Id_Inspeccion_Detalle));
            detalle.Comentario_MA = nuevaInspeccionDetalle.Comentario_MA;
            detalle.Estado = nuevaInspeccionDetalle.Estado;
            detalle.Fecha_Verificacion_MA = DateTime.Now;
            _context.Inspecciones_Detalle.Update(detalle);
            await _context.SaveChangesAsync();
        }
    }
}
