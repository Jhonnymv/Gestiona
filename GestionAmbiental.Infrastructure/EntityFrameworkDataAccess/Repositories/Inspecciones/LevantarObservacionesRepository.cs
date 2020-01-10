using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task LevantarObservacion(NuevaInspeccionDetalle nuevaInspeccionDetalle,int Id_Usuario)
        {
            var detalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x=>x.Id_Inspeccion_Detalle.Equals(nuevaInspeccionDetalle.IdInspeccionDetalle));
            detalle.Evidencia_Implementacion = nuevaInspeccionDetalle.EvidenciaImplementacion;
            detalle.Comentario_Implementacion = nuevaInspeccionDetalle.ComentarioImplementacion;
            detalle.Estado = EstadosInspeccion.Implementado.ToString();
            detalle.Fecha_Cierre = DateTime.Now;
            _context.Inspecciones_Detalle.Update(detalle);
            await _context.SaveChangesAsync();

            var log = new Log_Inspeccion();
            var bkp = JsonConvert.SerializeObject(detalle);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Inspeccion_Cabecera = detalle.Id_Inspeccion_Cabecera;
            log.Id_Usuario = Id_Usuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();

        }
    }
}
