using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora
{
    public class EspecieFloraOutput
    {
        public int Id_Especie_Flora { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public EspecieFloraOutput()
        {
        }
        public EspecieFloraOutput(Domain.Incidentes.Items.Especie_Flora especie)
        {
            Id_Especie_Flora = especie.Id_Especie_Flora.Value;
            Descripcion = especie.Descripcion;
            Estado = especie.Estado;
        }
    }
}
