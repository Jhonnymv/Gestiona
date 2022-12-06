using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.TipoCompromisoCMCB.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<TipoCompromisosOuput> Execute();
    }
}
