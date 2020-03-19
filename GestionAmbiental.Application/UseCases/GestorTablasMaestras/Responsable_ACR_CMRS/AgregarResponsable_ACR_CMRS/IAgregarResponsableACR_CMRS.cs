using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.AgregarResponsable_ACR_CMRS
{
    public interface IAgregarResponsableACR_CMRS
    {
        Task<int> Execute(string Nombre, string Apellido);
    }
}
