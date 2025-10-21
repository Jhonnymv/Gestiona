using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.Reportes
{
    public class ReporteIncidentes
    {
        public List<Dimension> FlashReports { get; set; }
        public List<Dimension> IncidentesMenores { get; set; }
        public List<Dimension> IncidentesMayores { get; set; }
    }
}
