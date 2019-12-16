using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTipoCondicion
{
    public interface IGetTipoCondicion
    {
        Task<TipoCondicionOutput> Execute(int idTipoCondicion);
    }
}
