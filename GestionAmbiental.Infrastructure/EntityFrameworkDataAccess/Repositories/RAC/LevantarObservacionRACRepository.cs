using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class LevantarObservacionRACRepository : ILevantarObservacionRACReadOnlyRepository
    {
        private readonly Context _context;
        public LevantarObservacionRACRepository(Context context)
        {
            _context = context;
        }
        public async Task LevantarObservacionRAC(Domain.RAC.RAC reporte, int IdUsuario)
        {
            var rac = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(reporte.Id_RAC));
            rac.Evidencia_Cierre = reporte.Evidencia_Cierre;
            rac.Comentario_Cierre = reporte.Comentario_Cierre;
            rac.Estado = EstadosRAC.Implementado.ToString();
            _context.RACs.Update(rac);
            await _context.SaveChangesAsync();
            
            var log = new Log_RAC();
            var bkp = JsonConvert.SerializeObject(rac);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_RAC = rac.Id_RAC;
            log.Id_Usuario = IdUsuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Log_RACs.Add(log);
            await _context.SaveChangesAsync();
            
        }
    }
}
