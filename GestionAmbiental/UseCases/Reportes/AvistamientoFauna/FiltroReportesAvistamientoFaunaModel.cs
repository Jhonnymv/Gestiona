using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.AvistamientoFauna
{
    public class FiltroReporteAvistamientoFaunaModel
    {
        [DisplayName("Nombre Comun:")]
        public int NombreComun { get; set; }
        [DisplayName("Tipo de Avistamiento:")]
        public int TipoAvistamiento { get; set; }
        [DisplayName("Estado de Fauna:")]
        public int EstadoFauna { get; set; }
        [DisplayName("Lugar de Referencia:")]
        public int LugarReferencia { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
    }
}
