using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS
{
    public class ManifiestosRSOutput
    {
        public List<ManifiestoRSOutput> manifiestosRS { get; set; }
        public int nManifiesto { get; set; }
    }
}
