using GestionAmbiental.Application.Repositories.ConsumoAgua;
using GestionAmbiental.Domain.ConsumoAgua;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ConsumoAgua
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<OrigenAgua>> GetOrigenesAgua()
        {
            var data = await _context.Origenes_Agua.ToListAsync();
            var output = new List<OrigenAgua>();
            foreach(var item in data)
            {
                output.Add(new OrigenAgua
                {
                    IdOrigenAgua = item.Id_Origen_Agua,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                });
            }
            return output;
        }

        public async Task<List<ResolucionConsumoAgua>> GetReslucionesConsumoAgua()
        {
            var data = await _context.Resoluciones_Consumo_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.ToListAsync();
            var origenes = await _context.Origenes_Agua.ToListAsync();

            var output = new List<ResolucionConsumoAgua>();
            foreach (var item in data)
            {
                var fuente = fuentes.FirstOrDefault(x => x.Id_Fuente_Agua.Equals(item.Id_Fuente_Agua));
                var origen = origenes.FirstOrDefault(x => x.Id_Origen_Agua.Equals(fuente.Id_Origen_Agua));

                output.Add(new ResolucionConsumoAgua { 
                    IdResolucionConsumoAgua = item.Id_Resolucion_Consumo_Agua,
                    Anio = item.Anio,
                    TipoUso = item.Tipo_Uso,
                    ClaseDerecho = item.Clase_Derecho,
                    Resolucion = item.Resolucion,
                    FuenteAgua = new FuenteAgua
                    {
                        IdFuenteAgua = fuente.Id_Fuente_Agua,
                        Descripcion = fuente.Descripcion,
                        Coordenadas_UTM = fuente.Coordenadas_UTM,
                        OrigenAgua = new OrigenAgua
                        {
                            IdOrigenAgua = origen.Id_Origen_Agua,
                            Descripcion = origen.Descripcion,
                            Estado = origen.Estado
                        },
                        Estado = fuente.Estado
                    },
                    VolumenOtorgado = item.Volumen_Otorgado,
                    LimiteDiario = item.Limite_Diario,
                    LimiteSemanal = item.Limite_Semanal,
                    LimiteMensual = item.Limite_Mensual,
                    Actividad = item.Actividad,
                    Estado = item.Estado
                });
            }
            return output;
        }
    }
}
