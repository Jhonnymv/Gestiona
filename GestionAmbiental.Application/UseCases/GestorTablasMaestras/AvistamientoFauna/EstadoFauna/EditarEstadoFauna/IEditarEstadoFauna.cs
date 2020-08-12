using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.EditarEstadoFauna
{
    public interface IEditarEstadoFauna
    {
        Task Execute(int idEstadoFauna, string descripcion, string estado);
    }
}
