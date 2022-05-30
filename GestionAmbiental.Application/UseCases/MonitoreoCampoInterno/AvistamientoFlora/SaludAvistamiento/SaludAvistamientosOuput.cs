using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento
{
    public class SaludAvistamientosOuput
    {
        public List<SaludAvistamientoOuput> Registros { get; set; }
        public int NRegistros { get; set; }
    }
}
