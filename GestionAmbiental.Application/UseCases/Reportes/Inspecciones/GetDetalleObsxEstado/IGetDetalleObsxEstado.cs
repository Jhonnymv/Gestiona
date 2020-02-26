using GestionAmbiental.Domain.Inspecciones;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObsxEstado
{
    public interface IGetDetalleObsxEstado
    {
        Task<List<DetalleObsxEstadoOutput>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado);
    }
}
