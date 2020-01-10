using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IVerificarObservacionRACReadOnlyRepository
    {
        Task VerificarObservacionRAC(Domain.RAC.RAC reporte, int IdUsuario,EstadosRAC nuevo_estado);
    }
}
