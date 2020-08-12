using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadoFauna
{
    public interface IGetEstadoFauna
    {
        Task<EstadoFaunaOutput> Execute(int idEstadoFauna);
    }
}
