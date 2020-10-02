using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaMar
{
    public class FiltroReportesConsumoAguaMarModel
    {
        [DisplayName("Resolución:")]
        public int Resolucion { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
    }
}
