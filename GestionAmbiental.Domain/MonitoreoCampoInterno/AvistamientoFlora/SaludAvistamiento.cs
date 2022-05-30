using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class SaludAvistamiento
    {
        public int Id { get; set; }
        public AvistamientoFlora Avistamiento { get; set; }
        public Salud Salud { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
