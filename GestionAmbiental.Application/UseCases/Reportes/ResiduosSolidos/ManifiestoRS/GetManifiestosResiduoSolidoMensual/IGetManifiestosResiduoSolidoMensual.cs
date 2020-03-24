using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosResiduoSolidoMensual
{
    public interface IGetManifiestosResiduoSolidoMensual
    {
        Task<List<DimensionOutput>> Execute(string FechaInicio, string FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada);
    }
}
