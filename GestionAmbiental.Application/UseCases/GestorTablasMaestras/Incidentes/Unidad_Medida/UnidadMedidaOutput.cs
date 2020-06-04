using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida
{
    public class UnidadMedidaOutput
    {
        public int Id_Unidad_Medida { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public UnidadMedidaOutput()
        {
        }
        public UnidadMedidaOutput(Domain.Incidentes.Items.Unidad_Medida unidad)
        {
            Id_Unidad_Medida = unidad.Id_Unidad_Medida.Value;
            Descripcion = unidad.Descripcion;
            Tipo = unidad.Tipo;
            Estado = unidad.Estado;
        }
    }
}
