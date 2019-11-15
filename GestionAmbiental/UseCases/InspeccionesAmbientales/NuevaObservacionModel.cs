using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class NuevaObservacionModel
    {
        public int idCabecera { get; set; }
        [DisplayName("Tipo de Inspección:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string TipoInspeccion { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Asesor_MA_Seguridad { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public int TipoCondicion { get; set; }
        [DisplayName("Evidencia:")]
        public List<IFormFile> Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo del Aspecto Ambiental:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada:")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable Marcobre:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista:")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada:")]
        [Required(ErrorMessage = "Debe ingresar una fecha")]
        public string FechaProgramada { get; set; }

        public NuevaObservacionModel()
        {
        }
    }
}
