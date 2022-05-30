using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class NombreCientificoAvistamiento
    {
        public int Id { get; set; }
        public AvistamientoFlora Avistamiento { get; set; }
        public NombreCientifico NombreCientifico { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
