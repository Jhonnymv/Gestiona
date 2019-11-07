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
        [DisplayName("Área")]
        public string Area { get; set; }
        [DisplayName("Gerencia Inspeccionada Marcobre:")]
        [Range(1,99,ErrorMessage ="Debe seleccionar una opcción")]
        public int Id_Gerencia_Ejecutora { get; set; }
        [DisplayName("Gerencia Inspeccionada EPCM:")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Gerencia_EPCM { get; set; }
        [DisplayName("Empresa Contratista Marcobre:")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Empresa_Contratista { get; set; }
        [DisplayName("Empresa Contratista EPCM:")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Empresa_Contratista_EPCM { get; set; }
        [DisplayName("Fecha:")]
        [Required(ErrorMessage ="Debe ingresar una fecha")]
        public string Fecha_Inspeccion { get; set; }
        [DisplayName("Hora:")]
        [Required(ErrorMessage = "Debe ingresar una hora")]
        public string Hora_Inspeccion { get; set; }
        [DisplayName("Lugar Inspeccionado:")]
        public string Lugar_Inspeccionado { get; set; }
        [DisplayName("Responsable Lugar Inspeccionado:")]
        [Range(1,Int32.MaxValue,ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Responsable_Lugar { get; set; }
        [DisplayName("Responsable Medio Ambiente:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Responsable_MA { get; set; }
        [DisplayName("Inspector(es):")]
        public string Inspectores { get; set; }
        [DisplayName("Objetivo de la Inspección:")]
        public string Objetivo { get; set; }
        //public NuevaObservacionModel Observacion { get; set; }
    }
}
