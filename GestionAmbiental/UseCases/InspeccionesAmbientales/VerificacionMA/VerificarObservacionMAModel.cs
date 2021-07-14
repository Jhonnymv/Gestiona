using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.VerificacionMA
{
    public class VerificarObservacionMAModel
    {
        public int idObs { get; set; }
        public string EncryptedId { get; set; }
        [DisplayName("Tipo de Observación")]
        public string TipoInspeccion { get; set; }
        [DisplayName("Asesor MAP o SSO")]
        public string AsesorSeguridad { get; set; }
        public int idCabecera { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar")]
        public string TipoCondicion { get; set; }
        [DisplayName("Evidencia Observación")]
        public List<string> Evidencia { get; set; }
        [DisplayName("Nivel de Riesgo del Aspecto Ambiental")]
        public string NivelRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.")]
        public string ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada para levantar Obs.")]
        public string FechaProgramada { get; set; }
        [DisplayName("Evidencia Implementación")]
        public List<string> EvidenciasImplementacion { get; set; }
        [DisplayName("Comentario Implementación")]
        public string ComentarioImplementacion { get; set; }
        [DisplayName("Comentario Verificación")]
        public string ComentarioVerificacionMA { get; set; }
        public string Accion { get; set; }
    }
}
