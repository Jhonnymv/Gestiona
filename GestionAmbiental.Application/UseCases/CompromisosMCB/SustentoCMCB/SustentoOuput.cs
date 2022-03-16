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

        public SustentoOuput()
        {
        }
        public SustentoOuput(Domain.CompromisosMCB.SustentoCMCB sustentoCMCB)
        {
            Id = sustentoCMCB.Id;
            IdCompromiso = sustentoCMCB.CompromisoCMCB.Id;
            Fecha_limite = sustentoCMCB.Fecha_limite;
            Fecha_entrega = sustentoCMCB.Fecha_entrega;
            Fecha_validacion = sustentoCMCB.Fecha_validacion;
            IdResponsable1 = sustentoCMCB.Responsable1.IdUsuario;
            IdResponsable2 = sustentoCMCB.Responsable2.IdUsuario;
            IdAsesor = sustentoCMCB.Asesor.IdUsuario;
            Estado = sustentoCMCB.Estado;
            Sustento_cumplimiento = sustentoCMCB.Sustento_cumplimiento;
            Detalle_compromiso = sustentoCMCB.Detalle_compromiso;
            Evidencia = sustentoCMCB.Evidencia;
            Comentario_implementacion = sustentoCMCB.Comentario_implementacion;
            Comentario_verificacion = sustentoCMCB.Comentario_verificacion;
            Medio_verificacion = sustentoCMCB.Medio_verificacion;
        }
    }
}
