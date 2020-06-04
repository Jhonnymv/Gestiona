using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna
{
    public class EspecieFaunaOutput
    {
        public int Id_Especie_Fauna { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public EspecieFaunaOutput() { }
        public EspecieFaunaOutput(Domain.Incidentes.Items.Especie_Fauna especie) {
            Id_Especie_Fauna = especie.Id_Especie_Fauna.Value;
            Descripcion = especie.Descripcion;
            Estado = especie.Estado;
        }

    }
}
