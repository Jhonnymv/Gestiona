using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.DetalleRegistro
{
    public interface IDetalleRegistro
    {
        Task<EtapaOuput> Execute(int Id);
    }
}