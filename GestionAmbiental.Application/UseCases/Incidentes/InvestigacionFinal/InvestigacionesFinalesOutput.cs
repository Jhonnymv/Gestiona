using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal
{
    public class InvestigacionesFinalesOutput
    {
        public List<InvestigacionFinalOutput> Investigaciones { get; set; }
        public int NInvestigaciones { get; set; }
        public bool PermisoAdmin { get; set; }
    }
}
