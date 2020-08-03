using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IRegistrarAvistamientoFaunaReadOnlyRepository
    {
        Task<int> RegistrarAvistamientoFauna(Domain.AvistamientoFauna.AvistamientoFauna avistamiento);
    }
}
