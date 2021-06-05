using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Registro
{
    public class EditarObservacionModel
    {
        public int E_IdCabecera { get; set; }
        public int E_IdObservacion { get; set; }
        public string E_IdObservacionEncryp { get; set; }
        [DisplayName("Tipo de Observación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string E_TipoInspeccion { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int E_AsesorSeguridad { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string E_CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public int E_TipoCondicion { get; set; }
        [DisplayName("Evidencia Cargada:")]
        public List<string> E_Evidencia { get; set; }
        [DisplayName("Nueva Evidencia (opcional):")]
        public List<IFormFile> E_Nueva_Evidencia { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string E_NivelRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string E_AccionTomada { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int E_ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string E_ResponsableCont { get; set; }
        [DisplayName("Fecha Programada para levantar Obs.:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string E_FechaProgramada { get; set; }
    }
}
