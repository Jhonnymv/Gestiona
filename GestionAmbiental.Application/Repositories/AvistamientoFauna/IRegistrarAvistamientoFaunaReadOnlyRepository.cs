using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IRegistrarAvistamientoFaunaReadOnlyRepository
    {
        Task<int> RegistrarAvistamientoFauna(int idUsuario, Domain.AvistamientoFauna.AvistamientoFauna avistamiento);
        Task<string> GetUltimoCodigo();
    }
}
