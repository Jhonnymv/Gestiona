using GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada
{
    public class ObservacionImplementadaOutput : NuevaObservacionOutput
    {
        public string FechaCierre { get; set; }
        public string ComentarioImplementacion { get; set; }
        public List<string> EvidenciaImplementacion { get; set; }
        public string EmailAsesor { get; set; }

    }
}
