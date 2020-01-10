using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface ILevantarObservacionRACReadOnlyRepository
    {
        Task LevantarObservacionRAC(Domain.RAC.RAC reporte, int IdUsuario);
    }
}
