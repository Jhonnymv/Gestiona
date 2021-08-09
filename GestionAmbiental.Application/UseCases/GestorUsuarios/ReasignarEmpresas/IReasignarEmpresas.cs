using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.ReasignarEmpresas
{
    public interface IReasignarEmpresas
    {
        Task<string> Execute();
    }
}
