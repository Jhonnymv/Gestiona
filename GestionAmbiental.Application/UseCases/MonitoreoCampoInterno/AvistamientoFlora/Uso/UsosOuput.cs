using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico
{
    public class NombreCientificosOuput
    {
        public List<SaludOuput> Registros { get; set; }
        public int NRegistros { get; set; }
    }
}
