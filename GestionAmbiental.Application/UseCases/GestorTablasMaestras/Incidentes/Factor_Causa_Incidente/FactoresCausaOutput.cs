using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente
{
    public class FactoresCausaOutput
    {
        public List<FactorCausaOutput> Factores { get; set; }
        public int NFactores { get; set; }
    }
}
