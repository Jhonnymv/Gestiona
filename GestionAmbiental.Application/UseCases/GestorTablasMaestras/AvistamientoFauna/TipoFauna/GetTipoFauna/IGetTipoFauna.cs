using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTipoFauna
{
    public interface IGetTipoFauna
    {
        Task<TipoFaunaOutput> Execute(int idTipoFauna);
    }
}
