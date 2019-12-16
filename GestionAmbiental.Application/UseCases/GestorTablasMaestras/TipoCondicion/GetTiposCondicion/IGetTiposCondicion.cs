using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTiposCondicion
{
    public interface IGetTiposCondicion
    {
        Task<TiposCondicionOutput> Execute(string filtro, int pagina, int rpp);
    }
}
