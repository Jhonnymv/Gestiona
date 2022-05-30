using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class FenologiaAvistamiento
    {
        public int Id { get; set; }
        public AvistamientoFlora Avistamiento { get; set; }
        public Fenologia Fenologia { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
