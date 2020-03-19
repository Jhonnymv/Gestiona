using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRSs
{
    public interface IGetACR_CMRSs
    {
        Task<ACR_CMRSsOutput> Execute(string filtro, int pagina, int rpp);
    }
}
