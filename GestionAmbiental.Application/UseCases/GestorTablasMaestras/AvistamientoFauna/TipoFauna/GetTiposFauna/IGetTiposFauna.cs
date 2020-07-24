using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTiposFauna
{
    public interface IGetTiposFauna
    {
        Task<TiposFaunaOutput> Execute(string filtro, int pagina, int rpp);
        Task<TiposFaunaOutput> Execute();
    }
}
