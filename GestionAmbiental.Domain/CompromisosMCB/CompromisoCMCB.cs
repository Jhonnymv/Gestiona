using System;
using System.Collections.Generic;
using System.Text;
using GestionAmbiental.Domain.ValueObjects;

namespace GestionAmbiental.Domain.CompromisosMCB
{
    public class CompromisoCMCB
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public CriterioCMCB CriterioCMCB { get; set; }
        public string? Detalle_compromiso { get; set; }
        public EtapaCMCB? EtapaCMCB { get; set; }
        public string? Texto_compromiso { get; set; }
        public OrigenCMCB? OrigenCMCB { get; set; }
        public string? Tomo { get; set; }
        public string? Capitulo { get; set; }
        public string? Seccion { get; set; }
        public string? Pagina { get; set; }
        public string? Resolucion { get; set; }
        public AutoridadCMCB? AutoridadCMCB { get; set; }
        public DateTime Fecha { get; set; }
        public CompromisoCMCB? Compromiso { get; set; } 
        public AspectoCMCB? AspectoCMCB { get; set; }
        public ComponenteCMCB? ComponenteCMCB { get; set; }
        public GerenciaCMCB? GerenciaCMCB { get; set; } 
        public AreaCMCB? AreaCMCB { get; set; }
        public string? Frecuencia { get; set; }
        //public Entidad
        public Usuario Responsable1 { get; set; }
        public Usuario Responsable2 { get; set; }
        public string? Frecuencia_reporte { get; set; }
        public string? Sustentos { get; set; }
        public string? Estado { get; set; }
        public Usuario Aprueba { get; set; }
        public string? Medio_verificacion { get; set; }

        //last added

        public string? Ano { get; set; }
        public string? Duplicado { get; set; }
        public TipoCompromisoCMCB? TipoCompromiso { get; set; }
        public string? Actividad { get; set; }
        public ComponenteCMCB? ComponenteAux { get; set; }
        public FactorAmbientalCMCB? FactorAmbiental { get; set; }
        public string? Vigencia { get; set; }
        public string? Motivo { get; set; }
        public string? Comentario { get; set; }


        public CompromisoCMCB()
        {

        }

    }
}
