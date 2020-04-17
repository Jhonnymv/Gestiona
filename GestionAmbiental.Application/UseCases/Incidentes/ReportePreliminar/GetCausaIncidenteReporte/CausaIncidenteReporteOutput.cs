using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte
{
    public class CausaIncidenteReporteOutput
    {
        public int Id_Causa_Incidente { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public int? Id_Acto_Condicion { get; set; }
        public string Acto_Condicion { get; set; }
        public string Tipo_Acto_Condicion { get; set; }
        public string Estado { get; set; }
    }
}
