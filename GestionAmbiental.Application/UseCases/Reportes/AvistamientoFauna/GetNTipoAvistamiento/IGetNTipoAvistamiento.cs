using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNTipoAvistamiento
{
    public interface IGetNTipoAvistamiento
    {
        Task<List<DimensionOutput>> Execute(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string FechaInicio, string FechaFin);
    }
}
