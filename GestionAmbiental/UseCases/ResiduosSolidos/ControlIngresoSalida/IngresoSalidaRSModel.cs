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
        public int Id_Ingreso_Salida_RS { get; set; }
        public string Encrypted_Id { get; set; }
        public string opcion { get; set; }
        [DisplayName("ACR/CMRS:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int ACR_CMRS { get; set; }
        [DisplayName("Responsable del ACR/CMRS que recibe/entrega el residuo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Responsable_ACR_CMRS { get; set; }
        [DisplayName("Ingreso o Salida de Residuos:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Ingreso_Salida { get; set; }
        [DisplayName("Motivo de Ingreso o Salida de Residuos:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Motivo { get; set; }
        [DisplayName("Fecha de Ingreso o Salida:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string Fecha { get; set; }
        [DisplayName("Tipo de Residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Tipo_Residuo { get; set; }
        [DisplayName("Nombre del Residuo:")]
        [Required(ErrorMessage = "Debe seleccionar una opción")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Residuo_Solido { get; set; }
        [DisplayName("Cantidad Ingresada o Retirada (kg):")]
        [Required(ErrorMessage = "Debe completar este campo")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public double Cantidad { get; set; }
        [DisplayName("Empresa que generó/retiró el residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Empresa { get; set; }
        [DisplayName("Responsable de Marcobre que entrega/retira el residuo:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Responsable_Marcobre { get; set; }
        [DisplayName("Comentarios:")]
        [Required(ErrorMessage = "Debe completar este campo")]
        public string Comentario { get; set; }
        public bool Permiso { get; set; }
        public bool Pendiente { get; set; }
    }
}
