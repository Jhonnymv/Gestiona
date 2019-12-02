using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Reportes
{
    public class ObservacionesUsuario
    {
        public Usuario usuario {get;set;}
        public int pendientes { get; set; }
        public int implementadas { get; set; }
        public int aprobadas { get; set; }
        public int rechazadas { get; set; }

    }
}
