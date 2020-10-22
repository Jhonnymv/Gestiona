using GestionAmbiental.Application.Repositories.ConsumoAgua;
using Microsoft.EntityFrameworkCore;
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
            var posteriores = await _context.Consumos_Agua
                .Where(x =>
                x.Id_Resolucion_Consumo_Agua.Equals(consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua) &&
                x.Lectura_Inicial > consumoAgua.LecturaInicial)
                .OrderBy(x => x.Lectura_Final)
                .ToListAsync();

            var entity = await _context.Consumos_Agua.FindAsync(consumoAgua.IdConsumoAgua);
            var variacion = consumoAgua.LecturaFinal - entity.Lectura_Final;

            entity.Id_Resolucion_Consumo_Agua = consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua;
            entity.Fecha = consumoAgua.Fecha;
            entity.Tiempo_Bombeo = consumoAgua.TiempoBombeo;
            //entity.Lectura_Inicial = consumoAgua.LecturaInicial;
            entity.Lectura_Final = consumoAgua.LecturaFinal;
            entity.Volumen_Bombeado = consumoAgua.VolumenBombeado;
            entity.Caudal_Promedio = consumoAgua.CaudalPromedio;
            entity.Estado = consumoAgua.Estado;

            _context.Consumos_Agua.Update(entity);

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
            _context.UpdateRange(posteriores);

            var nuevosConsumos = await _context.Consumos_Agua
                .Where(x => x.Id_Resolucion_Consumo_Agua.Equals(consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua))
                .OrderBy(x => x.Lectura_Final)
                .ToListAsync();

            await _context.SaveChangesAsync();
        }
    }
}
