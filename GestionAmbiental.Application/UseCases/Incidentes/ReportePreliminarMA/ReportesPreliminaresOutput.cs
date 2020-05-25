using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA
{
    public class ReportesPreliminaresOutput
    {
        public List<ReportePreliminarOutput> Reportes { get; set; }
        public int NReportes { get; set; }
    }
}
