using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.ProductoDerramado
{
    public interface IProductoDerramadoInvestigacion
    {
        Task<List<Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente>> Execute(string startTime, string endTime);
    }
}
