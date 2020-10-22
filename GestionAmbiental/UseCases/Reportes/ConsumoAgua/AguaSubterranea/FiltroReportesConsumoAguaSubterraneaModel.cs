using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaSubterranea
{
    public class FiltroReportesConsumoAguaSubterraneaModel
    {
        [DisplayName("Actividad:")]
        public int Resolucion { get; set; }
        [DisplayName("Año (Descarga Excel):")]
        public int Anio { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
    }
}
