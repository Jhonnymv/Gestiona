using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Verificacion
{
    public class VerificarObservacionModel
    {
        public int idObservacion { get; set; }
        public int idCabecera { get; set; }
        [DisplayName("Acto/Condición Subestándar Identificada:")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar:")]
        public string TipoCondicion { get; set; }
        [DisplayName("Evidencia:")]
        public string Evidencias { get; set; }
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
        public string EvidenciasInspeccion { get; set; }
        [DisplayName("Comentario Implementación:")]
        public string ComentarioImplementacion { get; set; }
        [DisplayName("Comentario Verificación:")]
        public string ComentarioVerificacion { get; set; }
        [DisplayName("Comentario Verificación Medio Ambiente:")]
        public string ComentarioVerificacionMA { get; set; }
    }
}
