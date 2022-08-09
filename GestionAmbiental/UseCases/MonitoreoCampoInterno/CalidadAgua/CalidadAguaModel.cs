using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAguaModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Fecha:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public DateTime Fecha { get; set; }

        [DisplayName("Hora:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Hora { get; set; }

        [DisplayName("Cod_ Estación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Cod_Estacion { get; set; }

        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }

        [DisplayName("Ubicación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion { get; set; }

        [DisplayName("Responsable:")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Debe Seleccionar un responsable")]
        public int Id_Usuario { get; set; }

        public string TemperaturaResult { get; set; }
        public string TemperaturaEquipo { get; set; }
        public string PhResult { get; set; }
        public string PhEquipo { get; set; }
        public string ConductividadResult { get; set; }
        public string ConductividadEquipo { get; set; }
        public string OxigenoResult { get; set; }
        public string OxigenoEquipo { get; set; }
        public string TurbiedadResult { get; set; }
        public string TurbiedadEquipo { get; set; }
        public string CloroResult { get; set; }
        public string CloroEquipo { get; set; }

    }
}
 