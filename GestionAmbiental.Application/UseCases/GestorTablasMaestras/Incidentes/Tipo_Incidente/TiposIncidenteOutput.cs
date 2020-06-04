using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente
{
    public class TiposIncidenteOutput
    {
        public List<TipoIncidenteOutput> Tipos { get; set; }
        public int NTipos { get; set; }
    }
}
