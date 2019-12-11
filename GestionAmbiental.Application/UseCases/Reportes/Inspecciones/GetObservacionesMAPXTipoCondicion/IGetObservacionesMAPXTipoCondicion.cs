using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesMAPXTipoCondicion
{
    public interface IGetObservacionesMAPXTipoCondicion
    {
        Task<List<List<Dimension>>> Execute();
    }
}
