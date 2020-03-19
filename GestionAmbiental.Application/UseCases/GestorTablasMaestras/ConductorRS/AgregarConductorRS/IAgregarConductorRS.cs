using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.AgregarConductorRS
{
    public interface IAgregarConductorRS
    {
        Task<int> Execute(string Nombres,string Apellidos, string NLicencia);
    }
}
