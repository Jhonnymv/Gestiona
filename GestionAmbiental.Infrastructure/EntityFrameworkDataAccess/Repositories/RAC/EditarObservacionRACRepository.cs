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
    public class EditarObservacionRACRepository : IEditarObservacionRACReadOnlyRepository
    {
        private readonly Context _context;

        public EditarObservacionRACRepository(Context context)
        {
            _context = context;
        }

        public async Task AmpliarFechaObservacionRAC(int idRAC, DateTime nuevaFecha,int idUsuario)
        {
            var rac_entity = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(idRAC));
            if (rac_entity != null)
            {
                rac_entity.Fecha_Cierre = nuevaFecha;
                rac_entity.Estado = EstadosRAC.Ampliado.ToString();
                _context.RACs.Update(rac_entity);
                await _context.SaveChangesAsync();

                var log = new Log_RAC();
                var bkp = JsonConvert.SerializeObject(rac_entity);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_RAC = rac_entity.Id_RAC;
                log.Id_Usuario = idUsuario;
                log.Tipo = TipoAccion.UPDATE.ToString();

                _context.Log_RACs.Add(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
