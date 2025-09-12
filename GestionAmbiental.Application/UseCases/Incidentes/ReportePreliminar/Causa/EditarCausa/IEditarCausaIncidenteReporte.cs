using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EditarCausa
{
    public interface IEditarCausaIncidenteReporte
    {
        Task<int> Execute(int Id_Causa_Incidente, int Id_Reporte, int Id_Acto_Condicion, string Tipo_Acto_Condicion, int Id_Factor_Causa, string Descripcion);
    }
}
