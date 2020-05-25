using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.RegistrarCausa
{
    public interface IRegistrarCausaIncidenteReporte
    {
        Task<int> Execute(int Id_Reporte, int Id_Acto_Condicion, string Tipo_Acto_Condicion);
    }
}
