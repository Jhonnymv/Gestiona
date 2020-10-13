using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ConsumoAgua
{
    public class ResolucionConsumoAgua
    {
        public int IdResolucionConsumoAgua { get; set; }
        public string Resolucion { get; set; }
        public string Anio { get; set; }
        public string TipoUso { get; set; }
        public string ClaseDerecho { get; set; }
        public FuenteAgua FuenteAgua { get; set; }
        public double VolumenOtorgado { get; set; }
        public double? LimiteDiario { get; set; }
        public double? LimiteSemanal { get; set; }
        public double? LimiteMensual { get; set; }
        public string Actividad { get; set; }
        public string Estado { get; set; }
    }
}
