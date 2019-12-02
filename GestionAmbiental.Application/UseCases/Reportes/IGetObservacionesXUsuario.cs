using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes
{
    public interface IGetObservacionesXUsuario
    {
        Task<List<Dimension>> Execute();
    }
}
