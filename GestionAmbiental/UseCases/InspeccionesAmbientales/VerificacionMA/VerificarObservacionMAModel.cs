using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.VerificacionMA
{
    public class VerificarObservacionMAModel
    {
        public int idObservacion { get; set; }
        public string EncryptedId { get; set; }
        [DisplayName("Tipo de Inspección:")]
        public string TipoInspeccion { get; set; }
        [DisplayName("Asesor MAP o SSO:")]
        public string Asesor_MA_Seguridad { get; set; }
        public int idCabecera { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        public string TipoCondicion { get; set; }
        [DisplayName("Evidencia:")]
        public List<string> Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo del Aspecto Ambiental:")]
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
        public List<string> EvidenciasInspeccion { get; set; }
        [DisplayName("Comentario Implementación:")]
        public string ComentarioImplementacion { get; set; }
        [DisplayName("Comentario Verificación:")]
        public string ComentarioVerificacionMA { get; set; }
        public string Accion { get; set; }
    }
}
