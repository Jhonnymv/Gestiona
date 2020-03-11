using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida
{
    public class FiltroIngresoSalidaModel
    {
        [DisplayName("ACR/CMRS:")]
        public int ACR_CMRS { get; set; }
        [DisplayName("Ingreso o Salida de Residuos:")]
        public int IngresoSalida { get; set; }
        [DisplayName("Tipo de Residuo:")]
        public int TipoResiduo { get; set; }
        [DisplayName("Motivo de Ingreso o Salida de Residuos:")]
        public int Motivo { get; set; }
        [DisplayName("Empresa que Generó/Retiró el Residuo:")]
        public int Empresa { get; set; }
        [DisplayName("Gerencia de Marcobre que Entrega/Retira el Residuo:")]
        public int Gerencia { get; set; }
        [DisplayName("Fecha de Inicio (Ingreso o Salida de Residuo):")]
        public string FInicio { get; set; }
        [DisplayName("Fecha de Fin (Ingreso o Salida de Residuo):")]
        public string FFin { get; set; }
    }
}
