using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.GetConductorRS
{
    public interface IGetConductorRS
    {
        Task<ConductorRSOutput> Execute(int idConductor);
    }
}
