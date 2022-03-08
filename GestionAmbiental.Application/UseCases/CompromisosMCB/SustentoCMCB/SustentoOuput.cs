using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB
{
    public class SustentoOuput
    {
        public int Id { get; set; }
        public int IdCompromiso { get; set; }
        public DateTime? Fecha_limite { get; set; }
        public DateTime? Fecha_entrega { get; set; }
        public DateTime? Fecha_validacion { get; set; }
        public int? IdResponsable1 { get; set; }
        public int? IdResponsable2 { get; set; }
        public int? IdAsesor { get; set; }
        public string? Estado { get; set; }
        public string? Sustento_cumplimiento { get; set; }
        public string? Detalle_compromiso { get; set; }
        public string? Evidencia { get; set; }
        public string? Comentario_implementacion { get; set; }
        public string? Comentario_verificacion { get; set; }
        public string? Medio_verificacion { get; set; }
    }
}
