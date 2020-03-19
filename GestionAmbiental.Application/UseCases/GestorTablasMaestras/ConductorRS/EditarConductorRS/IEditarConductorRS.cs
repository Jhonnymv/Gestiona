using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.EditarConductorRS
{
    public interface IEditarConductorRS
    {
        Task Execute(int IdConductor, string Nombres, string Apellidos, string NLicencia);
    }
}
