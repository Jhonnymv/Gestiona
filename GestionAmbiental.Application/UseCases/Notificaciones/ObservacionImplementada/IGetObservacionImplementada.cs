using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada
{
    public interface IGetObservacionImplementada
    {
        Task<ObservacionImplementadaOutput> Execute(int IdObservacion);
    }
}
