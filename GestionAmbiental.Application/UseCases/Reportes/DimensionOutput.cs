using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Reportes
{
    public class DimensionOutput
    {
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public int Index { get; set; }
        public DimensionOutput(Dimension dimension)
        {
            Nombre = dimension.Nombre;
            Valor = dimension.Valor;
            Index = dimension.Index;
        }
    }
}
