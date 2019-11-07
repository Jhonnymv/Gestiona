using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion
{
    public interface IGetNuevaObservacionInfo
    {
        Task<NuevaObservacionOutput> Execute(int IdObservacion);
    }
}
