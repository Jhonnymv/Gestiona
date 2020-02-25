using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlIngresoSalida
{
    public class IngresoSalidaRSModel
    {
        [DisplayName("ACR/CMRS:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int ACR_CMRS { get; set; }
        [DisplayName("Responsable del ACR/CMRS que recibe/entrega el residuo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Responsable_ACR_CMRS { get; set; }
        [DisplayName("Ingreso o Salida de Residuos:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Ingreso_Salida { get; set; }
        [DisplayName("Motivo de Ingreso o Salida de Residuos:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Motivo { get; set; }
        [DisplayName("Fecha de Ingreso o Salida:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string Fecha { get; set; }
        [DisplayName("Tipo de Residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Tipo_Residuo { get; set; }
        [DisplayName("Nombre del Residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Residuo_Solido { get; set; }
        [DisplayName("Cantidad Ingresada o Retirada (kg):")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public double Cantidad { get; set; }
        [DisplayName("Empresa que generó/retiró el residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Empresa { get; set; }
        
    }
}
