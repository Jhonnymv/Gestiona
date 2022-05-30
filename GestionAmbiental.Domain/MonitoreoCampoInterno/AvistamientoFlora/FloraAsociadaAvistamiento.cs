using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FloraAsociadaAvistamiento
    {
        public int Id { get; set; }
        public AvistamientoFlora Avistamiento { get; set; }
        public AvistamientoFlora FloraAsociada { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
