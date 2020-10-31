using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ConsumoAgua
{
    public class EditarConsumoAguaRepository : IEditarConsumoAguaReadOnlyRepository
    {
        private readonly Context _context;

        public EditarConsumoAguaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task EditarConsumoAgua(Domain.ConsumoAgua.ConsumoAgua consumoAgua)
        {
            var logNuevosConsumos = new List<Entities.ConsumoAgua.LogConsumoAgua>();
            var posteriores = await _context.Consumos_Agua
                .Where(x =>
                x.Id_Resolucion_Consumo_Agua.Equals(consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua) &&
                x.Lectura_Inicial > consumoAgua.LecturaInicial)
                .OrderBy(x => x.Lectura_Final)
                .ToListAsync();
            //Registro actual
            var entity = await _context.Consumos_Agua.FindAsync(consumoAgua.IdConsumoAgua);
            var variacion = consumoAgua.LecturaFinal - entity.Lectura_Final;

            entity.Id_Resolucion_Consumo_Agua = consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua;
            entity.Fecha = consumoAgua.Fecha;
            entity.Tiempo_Bombeo = consumoAgua.TiempoBombeo;
            entity.Lectura_Final = consumoAgua.LecturaFinal;
            entity.Volumen_Bombeado = consumoAgua.VolumenBombeado;
            entity.Caudal_Promedio = consumoAgua.CaudalPromedio;
            entity.Estado = consumoAgua.Estado;

            await _context.SaveChangesAsync();

            _context.Consumos_Agua.Update(entity);

            var log = new Entities.ConsumoAgua.LogConsumoAgua();
            var bkp = JsonConvert.SerializeObject(entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Consumo_Agua = entity.Id_Consumo_Agua;
            log.Id_Usuario = entity.Id_Usuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            logNuevosConsumos.Add(log);
            //Registros posteriores
            var nuevasEntidades = new List<Entities.ConsumoAgua.ConsumoAgua>();
            for (int i = 0; i < posteriores.Count; i++)
            {
                if (i == 0)
                {
                    posteriores[i].Lectura_Inicial = entity.Lectura_Final;
                    posteriores[i].Lectura_Final += variacion;
                }
                else
                {
                    posteriores[i].Lectura_Inicial = posteriores[i - 1].Lectura_Final;
                    posteriores[i].Lectura_Final += variacion;
                }
            }
            _context.Consumos_Agua.UpdateRange(posteriores);
            /*
            var nuevosConsumos = await _context.Consumos_Agua
                .Where(x => x.Id_Resolucion_Consumo_Agua.Equals(consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua))
                .OrderBy(x => x.Lectura_Final)
                .ToListAsync();
            */
            foreach (var consumo in posteriores)
            {
                log = new Entities.ConsumoAgua.LogConsumoAgua();
                bkp = JsonConvert.SerializeObject(consumo);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_Consumo_Agua = consumo.Id_Consumo_Agua;
                log.Id_Usuario = consumo.Id_Usuario;
                log.Tipo = TipoAccion.UPDATE.ToString();

                logNuevosConsumos.Add(log);
            }
            _context.Logs_Consumo_Agua.AddRange(logNuevosConsumos);

            await _context.SaveChangesAsync();
        }
    }
}
