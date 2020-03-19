using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.EditarACR_CMRS
{
    public interface IEditarACR_CMRS
    {
        Task Execute(int idACR_CMRS, string descripcion,string estado);
    }
}
