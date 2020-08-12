using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.GetAreasTrabajo
{
    public interface IGetAreasTrabajo
    {
        Task<AreasTrabajoOutput> Execute(string filtro, int pagina, int rpp);
    }
}
