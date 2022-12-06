using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB
{
    public class SustentoOuput
    {
        public int Id { get; set; }
        public int IdCompromiso { get; set; }
        public string Criterio { get; set; }
        public string Etapa { get; set; }
        public string Componente { get; set; }
        public string Gerencia { get; set; }
        public string Area { get; set; }
        public string Periodicidad { get; set; }
        public string Frecuencia_Reporte { get; set; }
        public string Entidad_Responsable { get; set; }
        public DateTime? Fecha_limite { get; set; }
        public DateTime? Fecha_entrega { get; set; }
        public DateTime? Fecha_validacion { get; set; }
        public int? IdResponsable1 { get; set; }
        public string Responsable1 { get; set; }
        public int? IdResponsable2 { get; set; }
        public string Responsable2 { get; set; }
        public int? IdAsesor { get; set; }
        public string? Asesor { get; set; }
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
            Criterio = sustentoCMCB.CompromisoCMCB.CriterioCMCB?.Criterio;
            Etapa = sustentoCMCB.CompromisoCMCB.EtapaCMCB.Etapa;
            Componente = sustentoCMCB.CompromisoCMCB.ComponenteCMCB.Componente;
            Gerencia = sustentoCMCB.CompromisoCMCB.GerenciaCMCB?.Gerencia ;
            Area = sustentoCMCB.CompromisoCMCB.AreaCMCB.Area;
            Periodicidad = sustentoCMCB.CompromisoCMCB.Frecuencia;
            Frecuencia_Reporte = sustentoCMCB.CompromisoCMCB.Frecuencia_reporte;
            Entidad_Responsable = sustentoCMCB.CompromisoCMCB.AutoridadCMCB.Autoridad;
            Fecha_limite = sustentoCMCB.Fecha_limite;
            Fecha_entrega = sustentoCMCB.Fecha_entrega;
            Fecha_validacion = sustentoCMCB.Fecha_validacion;
            IdResponsable1 = sustentoCMCB.Responsable1?.IdUsuario;
            Responsable1 = sustentoCMCB.Responsable1?.Nombres+" "+sustentoCMCB.Responsable1?.Apellidos;
            IdResponsable2 = sustentoCMCB.Responsable2?.IdUsuario;
            Responsable2 = sustentoCMCB.Responsable2?.Nombres + " " + sustentoCMCB.Responsable2?.Apellidos;
            IdAsesor = sustentoCMCB.Asesor?.IdUsuario;
            Asesor = sustentoCMCB.Asesor?.Nombres + " " + sustentoCMCB.Asesor?.Apellidos;
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
