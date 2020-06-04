using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinal.ImplementarAccion
{
    public class ImplementarAccionModel
    {
        public string Id_Encriptado { get; set; }
        public int Id_Accion_Incidente_Investigacion { get; set; }
        public int? Id_Investigacion_Final { get; set; }
        [DisplayName("Tipo de Acción:")]
        public string Tipo_Accion { get; set; }
        [DisplayName("Acción Inmediata:")]
        public string Accion_Inmediata { get; set; }
        [DisplayName("Responsable Marcobre:")]
        public string Responsable_Marcobre { get; set; }
        [DisplayName("Responsable Contratista:")]
        public string Responsable_Contratista { get; set; }
        [DisplayName("Fecha:")]
        public DateTime? Fecha { get; set; }
        [DisplayName("Evidencias de Implementación:")]
        public List<IFormFile> Evidencias { get; set; }
        [DisplayName("Estatus:")]
        public string Estatus { get; set; }
        public DateTime? Fecha_Implementacion { get; set; }
        [DisplayName("Comentarios:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Comentarios_Implementacion { get; set; }
        public string Estado { get; set; }
    }
}
