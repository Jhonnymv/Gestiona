using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Implementacion
{
    public class LevantarObservacionModel
    {

        public int idObservacion { get; set; }
        public int idCabecera { get; set; }
        [DisplayName("Tipo de Inspección:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opcción")]
        public string TipoInspeccion { get; set; }
        [DisplayName("Asesor MAP o SSO:")]
        public string Asesor_MA_Seguridad { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        public string TipoCondicion { get; set; }
        [DisplayName("Evidencia:")]
        public string Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada:")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable Marcobre:")]
        public string ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista:")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada:")]
        public string FechaProgramada { get; set; }
        [DisplayName("Evidencia:")]
        public List<IFormFile> EvidenciasInspeccion { get; set; }
        [DisplayName("Comentario:")]
        [Required(ErrorMessage ="Debe ingresar un comentario")]
        public string Comentario { get; set; }

        public LevantarObservacionModel()
        {
        }

    }
}
