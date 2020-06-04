using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente
{
    public class NivelesIncidenteOutput
    {
        public List<NivelIncidenteOutput> Niveles { get; set; }
        public int NNiveles { get; set; }
    }
}
