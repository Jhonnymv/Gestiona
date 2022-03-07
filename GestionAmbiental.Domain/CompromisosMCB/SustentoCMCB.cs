using System;
using System.Collections.Generic;
using System.Text;
using GestionAmbiental.Domain.ValueObjects;

namespace GestionAmbiental.Domain.CompromisosMCB
{
    public class SustentoCMCB
    {
        public int Id { get; set; }
        public CompromisoCMCB CompromisoCMCB { get; set; }
        public DateTime? Fecha_limite { get; set; }
        public DateTime? Fecha_entrega { get; set; }
        public DateTime? Fecha_validacion { get; set; }
        public Usuario? Responsable1 { get; set; }
        public Usuario? Responsable2 { get; set; }
        public Usuario? Asesor { get; set; }
        public string? Estado { get; set; }
        public string? Sustento_cumplimiento { get; set; }
        public string? Detalle_cumplimiento { get; set; }
        public string? Evidencia { get; set; }
        public string? Comentario_verificacion { get; set; }
        public string? Medio_verificacion { get; set; }

    }
}
