using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua
{
    public class ConsumoMensualOutput
    {
        public int Mes { get; set; }
        public double TiempoOperacion { get; set; }
        public double Caudal { get; set; }
        public double VolumenMensual { get; set; }
        public double VolumenAcumulado { get; set; }

    }
}
