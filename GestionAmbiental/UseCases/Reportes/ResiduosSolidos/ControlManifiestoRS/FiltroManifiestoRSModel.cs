using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.ResiduosSolidos.ControlManifiestoRS
{
    public class FiltroManifiestoRSModel
    {
        [DisplayName("Estado Manifiesto:")]
        public string Observacion { get; set; }
        [DisplayName("Residuo Sólido:")]
        public int Residuo_Solido { get; set; }
        [DisplayName("EO-RS Salida:")]
        public int EO_RS_Salida { get; set; }
        [DisplayName("EO-RS Llegada:")]
        public int EO_RS_Llegada { get; set; }
        [DisplayName("Fecha de Inicio (Fecha Emisión):")]
        public string FInicio { get; set; }
        [DisplayName("Fecha de Fin (Fecha Emisión):")]
        public string FFin { get; set; }
    }
}
