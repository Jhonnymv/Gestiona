using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IGetAvistamientoFaunaReadOnlyRepository
    {
        Task<List<Domain.AvistamientoFauna.AvistamientoFauna>> GetAvistamientosFauna(DateTime? fecha, int tipo_fauna, int estado_fauna, int lugar_referencia);
        Task<Domain.AvistamientoFauna.AvistamientoFauna> GetAvistamientoFauna(int id_avistamiento);
    }
}
