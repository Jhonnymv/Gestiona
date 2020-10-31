using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ConsumoAgua
{
    public class RegistrarConsumoAguaRepository : IRegistrarConsumoAguaReadOnlyRepository
    {
        private readonly Context _context;

        public RegistrarConsumoAguaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> RegistrarConsumoAgua(Domain.ConsumoAgua.ConsumoAgua consumo)
        {
            var entity = new Entities.ConsumoAgua.ConsumoAgua {
                Id_Resolucion_Consumo_Agua = consumo.ResolucionConsumoAgua.IdResolucionConsumoAgua,
                Fecha = consumo.Fecha,
                Tiempo_Bombeo = consumo.TiempoBombeo,
                Lectura_Inicial = consumo.LecturaInicial,
                Lectura_Final = consumo.LecturaFinal,
                Volumen_Bombeado = consumo.VolumenBombeado,
                Caudal_Promedio = consumo.CaudalPromedio,
                Id_Usuario = consumo.IdUsuario,
                Fecha_Registro = DateTime.Now,
                Estado = consumo.Estado
            };
            _context.Consumos_Agua.Add(entity);

            await _context.SaveChangesAsync();

            var log = new Entities.ConsumoAgua.LogConsumoAgua();
            var bkp = JsonConvert.SerializeObject(entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Consumo_Agua = entity.Id_Consumo_Agua;
            log.Id_Usuario = entity.Id_Usuario;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Logs_Consumo_Agua.Add(log);

            await _context.SaveChangesAsync();
        
            return entity.Id_Consumo_Agua;
        }

        public async Task<int> RegistrarConsumoAguaMasivo(List<Domain.ConsumoAgua.ConsumoAgua> consumos)
        {
            var nuevosConsumos = new List<Entities.ConsumoAgua.ConsumoAgua>();
            var logNuevosConsumos = new List<Entities.ConsumoAgua.LogConsumoAgua>();
            foreach (var consumo in consumos)
            {
                nuevosConsumos.Add(new Entities.ConsumoAgua.ConsumoAgua
                {
                    Id_Resolucion_Consumo_Agua = consumo.ResolucionConsumoAgua.IdResolucionConsumoAgua,
                    Fecha = consumo.Fecha,
                    Tiempo_Bombeo = consumo.TiempoBombeo,
                    Lectura_Inicial = consumo.LecturaInicial,
                    Lectura_Final = consumo.LecturaFinal,
                    Volumen_Bombeado = consumo.VolumenBombeado,
                    Caudal_Promedio = consumo.CaudalPromedio,
                    Id_Usuario = consumo.IdUsuario,
                    Fecha_Registro = DateTime.Now,
                    Estado = consumo.Estado
                });

            }
            _context.Consumos_Agua.AddRange(nuevosConsumos);
            
            await _context.SaveChangesAsync();
            foreach (var consumo in nuevosConsumos)
            {
                var log = new Entities.ConsumoAgua.LogConsumoAgua();
                var bkp = JsonConvert.SerializeObject(consumo);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_Consumo_Agua = consumo.Id_Consumo_Agua;
                log.Id_Usuario = consumo.Id_Usuario;
                log.Tipo = TipoAccion.CREATE.ToString();

                logNuevosConsumos.Add(log);
            }
            _context.Logs_Consumo_Agua.AddRange(logNuevosConsumos);

            await _context.SaveChangesAsync();

            return nuevosConsumos.Count;
        }
    }
}
