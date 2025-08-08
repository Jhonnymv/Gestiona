using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminar
{
    public class ReporteParte2Model
    {
        public int Id_Reporte_Preliminar { get; set; }
        [DisplayName("Otra Causa Inmediata")]
        public string Otra_Causa { get; set; }
        [DisplayName("Gerencia Responsable (Titular Minero)")]
        public int Id_Gerencia_Responsable { get; set; }
        [DisplayName("Comentario")]
        public string Comentario_Accion { get; set; }
        [DisplayName("Supervisor de Medio Ambiente encargado del seguimiento")]
        public int Id_Supervisor { get; set; }        
        [DisplayName("Evidencias")]
        public List<IFormFile> Evidencias { get; set; }
        public int NCausas { get; set; }
        public int NInvolucrados { get; set; }
        public int NAcciones { get; set; }
        public string Opcion { get; set; }
        public int? Id_Registrado_Por { get; set; }
    }
}
