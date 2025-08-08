using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport
{
    public class FlashReportsOutput
    {
        public List<FlashReportOutput> FlashReports { get; set; }
        public int NFlashReports{ get; set; }
        public bool PermisoAdmin { get; set; }
    }
}
