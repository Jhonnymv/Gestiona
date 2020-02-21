using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.RAC;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class VerificarObservacionRACRepository : IVerificarObservacionRACReadOnlyRepository
    {
        private readonly Context _context;
        public VerificarObservacionRACRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task VerificarObservacionRAC(Domain.RAC.RAC reporte, int IdUsuario,EstadosRAC nuevo_estado)
        {
            var rac = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(reporte.Id_RAC));
            rac.Comentario_Verificacion = reporte.Comentario_Verificacion;
            rac.Estado = nuevo_estado.ToString();
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
