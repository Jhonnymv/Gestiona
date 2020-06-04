using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente
{
    public class NivelIncidenteOutput
    {
        public int Id_Nivel_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public NivelIncidenteOutput()
        {
        }
        public NivelIncidenteOutput(Domain.Incidentes.Items.Nivel_Incidente nivel)
        {
            Id_Nivel_Incidente = nivel.Id_Nivel_Incidente.Value;
            Descripcion = nivel.Descripcion;
            Estado = nivel.Estado;
        }
    }
}
