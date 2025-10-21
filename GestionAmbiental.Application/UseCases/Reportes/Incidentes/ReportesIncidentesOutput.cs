using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes
{
    public class ReportesIncidentesOutput
    {
        public List<DimensionOutput> FlashReports { get; set; }
        public List<DimensionOutput> IncidentesMenores { get; set; }
        public List<DimensionOutput> IncidentesMayores { get; set; }
    }
}
