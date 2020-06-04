using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental
{
    public class ComponenteAmbientalOutput
    {
        public int Id_Componente_Ambiental { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ComponenteAmbientalOutput()
        {
        }

        public ComponenteAmbientalOutput(Domain.Incidentes.Items.Componente_Ambiental componente)
        {
            Id_Componente_Ambiental = componente.Id_Componente_Ambiental.Value;
            Descripcion = componente.Descripcion;
            Estado = componente.Estado;
        }
    }
}
