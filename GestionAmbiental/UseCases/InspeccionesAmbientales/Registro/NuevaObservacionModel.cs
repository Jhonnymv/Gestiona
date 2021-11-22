using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Registro
{
    public class NuevaObservacionModel
    {
        public int idCabecera { get; set; }
        public int IdObservacion { get; set; }
        [DisplayName("Tipo de Observación")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string TipoInspeccion { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int Asesor_MA_Seguridad { get; set; }
        [DisplayName("Acto/Condición Identificada")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Descripción de Acto/Condición")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string DescripcionActoCondicion { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar")]
        public int? TipoCondicion { get; set; }
        [DisplayName("Evidencia")]
        [Required(ErrorMessage = "Debe ingresar una evidencia")]
        public List<IFormFile> Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva")]
        public string AccionTomada { get; set; }
        [DisplayName("Resp. Marcobre levantar Obs.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opcción")]
        public int ResponsableMarc { get; set; }
        [DisplayName("Resp. SEcundario Marcobre levantar Obs")]
        public int? ResponsableMarc2 { get; set; }
        [DisplayName("Responsable Contratista")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string ResponsableCont { get; set; }
        [DisplayName("Correo Electrónico de Responsable Contratista")]
        public string CorreoCont { get; set; }
        [DisplayName("Fecha Programada para levantar Obs.")]
        public string FechaProgramada { get; set; }

        public NuevaObservacionModel()
        {
        }
    }
}
