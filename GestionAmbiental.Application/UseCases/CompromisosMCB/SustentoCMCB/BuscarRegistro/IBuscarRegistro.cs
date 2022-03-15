using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.BuscarRegistro
{
    public interface IBuscarRegistro
    {
        Task<SustentoOuput> Execute(int Id);
    }
}
