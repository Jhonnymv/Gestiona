using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class ResolucionConsumoAguaModel
    {
        public int IdResolucionConsumoAgua { get; set; }
        [DisplayName("Resolución:")]
        public string Resolucion { get; set; }
        [DisplayName("Año:")]
        public string Anio { get; set; }
        [DisplayName("Tipo de Uso:")]
        public string TipoUso { get; set; }
        [DisplayName("Clase de Derecho:")]
        public string ClaseDerecho { get; set; }
        [DisplayName("Fuente de Agua:")]
        public int IdFuenteAgua { get; set; }
        [DisplayName("Volumen Otorgado (m3):")]
        public double VolumenOtorgado { get; set; }
        [DisplayName("Limite Diario (m3):")]
        public double? LimiteDiario { get; set; }
        [DisplayName("Limite Semanal (m3):")]
        public double? LimiteSemanal { get; set; }
        [DisplayName("Limite Mensual (m3):")]
        public double? LimiteMensual { get; set; }
        [DisplayName("Fecha de Vencimiento:")]
        public string FechaVencimiento { get; set; }
        [DisplayName("Caudal (L/s):")]
        public double? Caudal { get; set; }
        [DisplayName("Actividad:")]
        public string Actividad { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
