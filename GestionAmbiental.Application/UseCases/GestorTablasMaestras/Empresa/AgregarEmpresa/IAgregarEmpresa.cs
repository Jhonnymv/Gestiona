using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.AgregarEmpresa
{
    public interface IAgregarEmpresa
    {
        Task<int> Execute(string razonSocial);
    }
}
