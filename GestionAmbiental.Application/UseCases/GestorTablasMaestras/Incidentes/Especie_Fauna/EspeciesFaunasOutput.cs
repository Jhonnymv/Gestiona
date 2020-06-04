using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna
{
    public class EspeciesFaunasOutput
    {
        public List<EspecieFaunaOutput> Especies { get; set; }
        public int NEspecies { get; set; }
    }
}
