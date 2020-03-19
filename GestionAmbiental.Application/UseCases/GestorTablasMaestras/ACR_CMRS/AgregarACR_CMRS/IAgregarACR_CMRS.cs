using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.AgregarACR_CMRS
{
    public interface IAgregarACR_CMRS
    {
        Task<int> Execute(string descripcion);
    }
}
