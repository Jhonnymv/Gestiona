using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente
{
    public class TipoIncidenteOutput
    {
        public int Id_Tipo_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoIncidenteOutput()
        {
        }
        public TipoIncidenteOutput(Domain.Incidentes.Items.Tipo_Incidente tipo)
        {
            Id_Tipo_Incidente = tipo.Id_Tipo_Incidente.Value;
            Descripcion = tipo.Descripcion;
            Estado = tipo.Estado;
        }
    }
}
