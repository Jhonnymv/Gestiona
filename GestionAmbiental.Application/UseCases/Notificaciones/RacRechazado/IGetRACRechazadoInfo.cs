using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.RacRechazado
{
    public interface IGetRACRechazadoInfo
    {
        Task<RacRechazadoOutput> Execute(int Id_RAC);
    }
}
