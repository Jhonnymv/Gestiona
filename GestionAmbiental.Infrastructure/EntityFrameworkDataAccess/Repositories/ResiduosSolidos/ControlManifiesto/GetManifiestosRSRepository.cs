using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiesto;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlManifiesto
{
    public class GetManifiestosRSRepository : IGetManifiestosRSReadOnlyRepository
    {
        private readonly Context _context;

        public GetManifiestosRSRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Manifiesto_RS>> GetManifiestosRS(string nManifiesto, DateTime? fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int asesorMAP)
        {
            var data = await _context.Manifiesto_RSs.Where(x =>
                x.Num_Manifiesto.Contains(nManifiesto!=""?nManifiesto:x.Num_Manifiesto) &&
                x.Fecha_Emision_Manifiesto.Equals(fechaEmision != null ? fechaEmision.Value : x.Fecha_Emision_Manifiesto) &&
                x.Observacion_Manifiesto.Equals(observacion!=""?observacion:x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuoSolido!=0?residuoSolido:x.Id_Residuo_Solido)&&
                x.Id_EO_RS_Salida.Equals(eors_salida!=0?eors_salida:x.Id_EO_RS_Salida)&&
                x.Id_EO_RS_Llegada.Equals(eors_llegada!=0? eors_llegada : x.Id_EO_RS_Llegada)&&
                x.Id_Asesor_MAP.Equals(asesorMAP!=0?asesorMAP:x.Id_Asesor_MAP)&&
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()))
                .OrderBy(x=>x.Num_Manifiesto)
                .OrderBy(x=>x.Serie_Manifiesto)
                .ToListAsync();

            var output = new List<Manifiesto_RS>();

            foreach(var item in data)
            {
                var outputItem = new Manifiesto_RS();
                outputItem.Serie_Manifiesto = item.Serie_Manifiesto;
                outputItem.Num_Manifiesto = item.Num_Manifiesto;

            }
            return output;
        }
    }
}
