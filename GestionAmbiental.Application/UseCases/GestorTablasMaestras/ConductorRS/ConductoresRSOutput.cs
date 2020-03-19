using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS
{
    public class ConductoresRSOutput
    {
        public List<ConductorRSOutput> Conductores { get; set; }
        public int NConductores { get; set; }
    }
}
