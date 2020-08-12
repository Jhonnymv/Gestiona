using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.GetAreaTrabajo
{
    public interface IGetAreaTrabajo
    {
        Task<AreaTrabajoOutput> Execute(int idEstadoFauna);
    }
}
