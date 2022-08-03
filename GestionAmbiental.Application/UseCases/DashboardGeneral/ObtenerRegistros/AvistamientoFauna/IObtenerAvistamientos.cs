using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.AvistamientoFauna
{
    public interface IObtenerAvistamientos
    {
        Task<List<Domain.AvistamientoFauna.AvistamientoFauna>> Execute(string startTime, string endTime);
    }
}
