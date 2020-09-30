using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaSubterranea
{
    public class FiltroReportesConsumoAguaSubterraneaModel
    {
        [DisplayName("Resolución:")]
        public int Resolucion { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
    }
}
