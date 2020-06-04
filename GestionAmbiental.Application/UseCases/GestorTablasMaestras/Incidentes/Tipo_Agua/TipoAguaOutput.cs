using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua
{
    public class TipoAguaOutput
    {
        public int Id_Tipo_Agua { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoAguaOutput()
        {
        }
        public TipoAguaOutput(Domain.Incidentes.Items.Tipo_Agua tipo_Agua)
        {
            Id_Tipo_Agua = tipo_Agua.Id_Tipo_Agua.Value;
            Descripcion = tipo_Agua.Descripcion;
            Estado = tipo_Agua.Estado;
        }
    }
}
