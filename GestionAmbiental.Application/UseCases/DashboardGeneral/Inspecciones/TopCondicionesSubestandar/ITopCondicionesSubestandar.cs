using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopCondicionesSubestandar
{
    public interface ITopCondicionesSubestandar
    {
        Task<InspeccionesMAPSSOOutput> Execute(int top);
    }
}
