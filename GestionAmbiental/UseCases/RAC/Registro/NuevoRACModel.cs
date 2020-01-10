using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.RAC.Registro
{
    public class NuevoRACModel
    {
        public int Id_RAC { get; set; }
        [DisplayName("Nombres y Apellidos:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Nombres_Apellidos { get; set; }
        [DisplayName("Área de Marcobre:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Gerencia { get; set; }
        [DisplayName("Empresa:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Empresa { get; set; }
        [DisplayName("Otra empresa:")]
        public string Otra_Empresa { get; set; }
        [DisplayName("Empresa observada:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Empresa_Obs { get; set; }
        [DisplayName("Gerencia a la que pertenece (Marcobre):")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Gerencia_Obs { get; set; }
        [DisplayName("Ubicación de la observación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion { get; set; }
        [DisplayName("Fecha:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string Fecha { get; set; }
        [DisplayName("Hora (24h):")]
        [Required(ErrorMessage = "Debe ingresar una hora")]
        public string Hora { get; set; }
        [DisplayName("Observación SSO/MA:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string SSO_MA { get; set; }
        [DisplayName("Clasificación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string Clasificacion { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string NivelRiesgo { get; set; }
        [DisplayName("Tipo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Tipo_Condicion { get; set; }
        [DisplayName("Descripción del acto o condición subestándar:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion_Acto_Cond { get; set; }
        [DisplayName("Medida(s) de control inmediata:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Medida_Control { get; set; }
        [DisplayName("¿Se planteó una primera acción correctiva?:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string PAC { get; set; }
        [DisplayName("Responsable de Marcobre del cierre de la observación:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Responsable_Cierre { get; set; }
        [DisplayName("Fecha de cierre programada:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string Fecha_Cierre { get; set; }
        [DisplayName("Supervisor de SSO/MA (Marcobre) a notificar:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Id_Supervisor { get; set; }
        [DisplayName("Evidencia de la Observación:")]
        [Required(ErrorMessage = "Debe ingresar una evidencia")]
        public IFormFile Evidencia_Obs { get; set; }
    }
}
