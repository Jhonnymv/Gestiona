using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsable_ACR_CMRS
{
    public interface IGetResponsableACR_CMRS
    {
        Task<ResponsableACR_CMRSOutput> Execute(int idResponsable);
    }
}
