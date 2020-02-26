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

        public int idCabecera { get; set; }
        public int IdObservacion { get; set; }
        public string IdObservacionEncryp { get; set; }
        [DisplayName("Tipo de Observación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string TipoInspeccion { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Asesor_MA_Seguridad { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public int TipoCondicion { get; set; }
        [DisplayName("Evidencia Cargada:")]
        public List<string> Evidencia { get; set; }
        [DisplayName("Nueva Evidencia (opcional):")]
        public List<IFormFile> Nueva_Evidencia { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada para levantar Obs.:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string FechaProgramada { get; set; }
    }
}
