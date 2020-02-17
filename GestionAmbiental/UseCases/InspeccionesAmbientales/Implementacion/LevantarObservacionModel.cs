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
        public string EncryptedId { get; set; }
        public int idCabecera { get; set; }
        [DisplayName("Tipo de Observación:")]
        public string TipoInspeccion { get; set; }
        [DisplayName("Asesor MAP o SSO:")]
        public string Asesor_MA_Seguridad { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        public string TipoCondicion { get; set; }
        [DisplayName("Evidencia:")]
        public List<string> Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada:")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.:")]
        public string ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista:")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada para levantar Obs.:")]
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
