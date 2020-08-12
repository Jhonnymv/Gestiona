using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.AgregarEstadoFauna
{
    public interface IAgregarEstadoFauna
    {
        Task<int> Execute(string descripcion);
    }
}
