using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class NuevaInspeccionModel
    {
        public string opcion { get; set; }
        public int Id_Inspeccion_Cabecera { get; set; }
        [DisplayName("Código")]
        public string Codigo { get; set; }
        [DisplayName("Área Ejecutora:")]
        [Range(1,99,ErrorMessage ="Debe seleccionar una opcción")]
        public int Id_Gerencia_Ejecutora { get; set; }
        [DisplayName("Empresa (inspeccionada):")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Empresa_Contratista { get; set; }
        [DisplayName("Fecha:")]
        [Required(ErrorMessage ="Debe ingresar una fecha")]
        public string Fecha_Inspeccion { get; set; }
        [DisplayName("Hora (24h):")]
        [Required(ErrorMessage = "Debe ingresar una hora")]
        public string Hora_Inspeccion { get; set; }
        [DisplayName("Área Inspeccionada:")]
        public string Lugar_Inspeccionado { get; set; }
        [DisplayName("Tipo de Inspección:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string Tipo_Inspeccion { get; set; }
        [DisplayName("Responsable del Área Inspeccionada:")]
        [Range(1,Int32.MaxValue,ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Responsable_Lugar { get; set; }
        [DisplayName("Responsable de la Inspección:")]
        public string Responsable_Inspeccion { get; set; }
        [DisplayName("Participantes de la Inspección:")]
        public string Inspectores { get; set; }
        [DisplayName("Objetivo de la Inspección:")]
        public string Objetivo { get; set; }
        //public NuevaObservacionModel Observacion { get; set; }
        public string Estado { get; set; }
    }
}
