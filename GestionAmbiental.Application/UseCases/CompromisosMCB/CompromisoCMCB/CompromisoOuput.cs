using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB
{
    public class CompromisoOuput
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int? IdCriterio { get; set; }
        public string? NomCriterio { get; set; }
        public string? Detalle_compromiso { get; set; }
        public int? IdEtapa { get; set; }
        public string? NomEtapa { get; set; }
        public string? Texto_compromiso { get; set; }
        public int? IdOrigen { get; set; }
        public string? NomOrigen { get; set; }
        public string? Tomo { get; set; }
        public string? Capitulo { get; set; }
        public string? Seccion { get; set; }
        public string? Pagina { get; set; }
        public string? Resolucion { get; set; }
        public int? IdAutoridad { get; set; }
        public string? NomAutoridad { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdCompromiso { get; set; }
        public string? NomCompromiso { get; set; }
        public int? IdAspecto { get; set; }
        public string? NomAspecto { get; set; }
        public int? IdComponente { get; set; }
        public string? NomComponente { get; set; }
        public int? IdGerencia { get; set; }
        public string? NomGerencia { get; set; }
        public int? IdArea { get; set; }
        public string? NomArea { get; set; }
        public string? Frecuencia { get; set; }
        public int? IdEntidad { get; set; }
        public int? IdResponsable1 { get; set; }
        public string? NomResponsable1 { get; set; }
        public int? IdResponsable2 { get; set; }
        public string? NomResponsable2 { get; set; }
        public string? Frecuencia_reporte { get; set; }
        public string? Sustentos { get; set; }
        public string? Estado { get; set; }
        public int? Aprueba { get; set; }
        public string? NomAprueba { get; set; }
        public string? Medio_verificacion { get; set; }

        //public CompromisoOuput(Domain.CompromisosMCB.CompromisoCMCB compromiso)
        //{
        //    Id = compromiso.Id;
        //    Codigo = compromiso.Codigo;
        //    IdCriterio = compromiso.CriterioCMCB.Id;
        //    NomCriterio = compromiso.CriterioCMCB.Criterio;

        //}
    }
}
