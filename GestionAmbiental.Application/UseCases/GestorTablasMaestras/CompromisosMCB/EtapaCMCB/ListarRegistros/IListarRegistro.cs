using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.ListarRegistros
{
    public interface IListarRegistro
    {
        Task<EtapasOuput> Execute(string filtro, int pagina, int rpp);
    }
}