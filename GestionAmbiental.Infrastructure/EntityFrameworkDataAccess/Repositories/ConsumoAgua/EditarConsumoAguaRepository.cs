using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
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
            var entity = await _context.Consumos_Agua.FindAsync(consumoAgua.IdConsumoAgua);

            entity.Id_Resolucion_Consumo_Agua = consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua;
            entity.Fecha = consumoAgua.Fecha;
            entity.Tiempo_Bombeo = consumoAgua.TiempoBombeo;
            entity.Lectura_Inicial = consumoAgua.LecturaInicial;
            entity.Lectura_Final = consumoAgua.LecturaFinal;
            entity.Volumen_Bombeado = consumoAgua.VolumenBombeado;
            entity.Caudal_Promedio = consumoAgua.CaudalPromedio;
            entity.Estado = consumoAgua.Estado;

            _context.Consumos_Agua.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
