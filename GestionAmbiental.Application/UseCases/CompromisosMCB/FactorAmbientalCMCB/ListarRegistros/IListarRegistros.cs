using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.FactorAmbientalCMCB.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<FactoresAmbientalesOuput> Execute();
    }
}
