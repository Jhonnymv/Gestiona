using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna
{
    public interface IGetAvistamientosFauna
    {
        Task<AvistamientosFaunaOutput> Execute(string fecha, int tipo_fauna, int estado_fauna,int lugar_referencia, int rpp, int pagina);
    }
}
