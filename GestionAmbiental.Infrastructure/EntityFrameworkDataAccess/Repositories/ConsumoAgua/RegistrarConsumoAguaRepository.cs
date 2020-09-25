using GestionAmbiental.Application.Repositories.ConsumoAgua;
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

            return entity.Id_Consumo_Agua;
        }

        public async Task<int> RegistrarConsumoAguaMasivo(List<Domain.ConsumoAgua.ConsumoAgua> consumos)
        {
            var nuevosConsumos = new List<Entities.ConsumoAgua.ConsumoAgua>();
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

            return nuevosConsumos.Count;
        }
    }
}
