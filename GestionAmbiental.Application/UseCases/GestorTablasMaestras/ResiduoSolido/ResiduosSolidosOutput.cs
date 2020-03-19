using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido
{
    public class ResiduosSolidosOutput
    {
        public List<ResiduoSolidoOutput> ResiduosSolidos { get; set; }
        public int NResiduos { get; set; }
    }
}
