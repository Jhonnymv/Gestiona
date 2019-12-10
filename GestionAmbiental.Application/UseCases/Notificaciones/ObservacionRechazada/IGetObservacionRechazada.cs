using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ObservacionRechazada
{
    public interface IGetObservacionRechazada
    {
        Task<ObservacionRechazadaOutput> Execute(int IdObservacion);
    }
}
