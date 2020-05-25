using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.Incidentes.ReportePreliminar
{
    public class FiltroReportesIncidentesReportePreliminarModel
    {
        [DisplayName("Tipo de Incidente:")]
        public int TipoIncidente { get; set; }
        [DisplayName("Nivel de Incidente:")]
        public int NivelIncidente { get; set; }
        [DisplayName("Gerencia:")]
        public int Gerencia { get; set; }
        [DisplayName("Supervisor de Medio Ambiente encargado del seguimiento:")]
        public int Supervisor { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
    }
}
