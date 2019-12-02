using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion
{
    public interface IAmpliarFechaObservacion
    {
        Task Execute(int idObservacion, string nuevaFecha, string usuario_red);
    }
}
