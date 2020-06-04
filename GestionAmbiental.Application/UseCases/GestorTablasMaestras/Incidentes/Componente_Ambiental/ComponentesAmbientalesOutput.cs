using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental
{
    public class ComponentesAmbientalesOutput
    {
        public List<ComponenteAmbientalOutput>Componentes { get; set; }
        public int NComponentes { get; set; }
    }
}
