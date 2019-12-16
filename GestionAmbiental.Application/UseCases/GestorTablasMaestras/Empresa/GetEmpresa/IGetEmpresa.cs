using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresa
{
    public interface IGetEmpresa
    {
        Task<EmpresaOutput> Execute(int idEmpresa);
    }
}
