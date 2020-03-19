using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsables_ACR_CMRS
{
    public interface IGetResponsablesACR_CMRS
    {
        Task<ResponsablesACR_CMRSOutput> Execute(string filtro, int pagina, int rpp);
    }
}
