using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRS
{
    public interface IGetACR_CMRS
    {
        Task<ACR_CMRSOutput> Execute(int idACR_CMRS);
    }
}
