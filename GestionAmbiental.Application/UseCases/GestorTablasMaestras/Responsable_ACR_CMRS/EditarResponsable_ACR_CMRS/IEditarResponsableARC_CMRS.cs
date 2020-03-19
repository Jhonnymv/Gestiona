using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.EditarResponsable_ACR_CMRS
{
    public interface IEditarResponsableARC_CMRS
    {
        Task Execute(int IdResponsable, string Nombre, string Apellido, string Estado);
    }
}
