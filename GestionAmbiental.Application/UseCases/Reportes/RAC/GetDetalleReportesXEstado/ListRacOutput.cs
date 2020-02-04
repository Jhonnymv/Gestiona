using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetDetalleReportesXEstado
{
    public class ListRacOutput
    {
        public List<RacOutput> racs { get; set; }
        public int nRacs { get; set; }
    }
}
