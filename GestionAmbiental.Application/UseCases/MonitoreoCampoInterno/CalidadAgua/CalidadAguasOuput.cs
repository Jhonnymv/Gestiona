using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAguasOuput
    {
        public List<CalidadAguaOuput> Registros { get; set; }

        public int NRegistros { get; set; }
    }
}
