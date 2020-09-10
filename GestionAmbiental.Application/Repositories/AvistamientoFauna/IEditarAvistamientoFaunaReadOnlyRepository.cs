using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IEditarAvistamientoFaunaReadOnlyRepository
    {
        Task EliminarFotoAvistamiento(int IdAvistamiento, string foto);
        Task ActualizarAvistamiento(int idUsuario, Domain.AvistamientoFauna.AvistamientoFauna nuevoAvistamientoFauna);
        Task CompletarAvistamiento(Domain.AvistamientoFauna.AvistamientoFauna avistamientoMA);
        Task EliminarAvistamiento(int idUsuario, int IdAvistamiento);
    }
}
