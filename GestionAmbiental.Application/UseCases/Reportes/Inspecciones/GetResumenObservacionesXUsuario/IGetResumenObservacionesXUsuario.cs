using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetResumenObservacionesXUsuario
{
    public interface IGetResumenObservacionesXUsuario
    {
        Task<List<ObservacionesUsuario>> Execute(string filtro);
    }
}
