using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.EditarEmpresa
{
    public interface IEditarEmpresa
    {
        Task Execute(int idEmpresa, string razonSocial);
    }
}
