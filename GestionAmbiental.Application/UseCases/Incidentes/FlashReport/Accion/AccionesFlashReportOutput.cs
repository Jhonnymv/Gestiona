using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion
{
    public class AccionesFlashReportOutput
    {
        public List<AccionFlashReportOutput> AccionesFlashReport { get; set; }
        public int NAccionesFlashReports { get; set; }
    }
}
