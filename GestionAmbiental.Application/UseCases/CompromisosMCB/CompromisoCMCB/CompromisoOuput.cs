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

        //last added

        public string Ano { get; set; }
        public string? Duplicado { get; set; }
        public int? Id_TipoCompromiso { get; set; }
        public string? Nomb_TipoCompromiso { get; set; }
        public string? Actividad { get; set; }
        public int? Id_ComponenteAux { get; set; }
        public string? Nomb_ComponenteAux { get; set; }
        public int? Id_FactorAmbiental { get; set; }
        public string? Nomb_FactorAmbiental { get; set; }
        public string? Vigencia { get; set; }
        public string? Motivo { get; set; }
        public string? Comentario { get; set; }

        public CompromisoOuput()
        {

        }

        public CompromisoOuput(Domain.CompromisosMCB.CompromisoCMCB compromiso)
        {
            Id = compromiso.Id;
            Codigo = compromiso.Codigo;
            IdCriterio = compromiso.CriterioCMCB.Id;
            NomCriterio = compromiso.CriterioCMCB.Criterio;
            Detalle_compromiso = compromiso.Detalle_compromiso;
            IdEtapa=compromiso.EtapaCMCB.Id;
            NomEtapa=compromiso.EtapaCMCB.Etapa;
            Texto_compromiso= compromiso.Texto_compromiso;
            IdOrigen=compromiso.OrigenCMCB.Id;
            NomOrigen = compromiso.OrigenCMCB.Origen;
            Tomo = compromiso.Tomo;
            Capitulo=compromiso.Capitulo;
            Seccion=compromiso.Seccion;
            Pagina =compromiso.Pagina;
            Resolucion =compromiso.Resolucion;
            IdAutoridad = compromiso.AutoridadCMCB.Id;
            NomAutoridad = compromiso.AutoridadCMCB.Autoridad;
            Fecha = compromiso.Fecha;
            IdCompromiso = compromiso.Compromiso.Id;
            NomCompromiso = compromiso.Compromiso.Codigo;
            IdAspecto = compromiso.AspectoCMCB.Id;
            NomAspecto = compromiso.AspectoCMCB.Aspecto;
            IdCompromiso = compromiso.ComponenteCMCB.Id;
            NomComponente = compromiso.ComponenteCMCB.Componente;
            IdGerencia = compromiso.GerenciaCMCB.Id;
            NomGerencia = compromiso.GerenciaCMCB.Gerencia;
            IdArea = compromiso.AreaCMCB.Id;
            NomArea = compromiso.AreaCMCB.Area;
            Frecuencia = compromiso.Frecuencia;

            IdResponsable1 = compromiso.Responsable1?.IdUsuario;
            NomResponsable1 = compromiso.Responsable1?.Nombres + " " + compromiso.Responsable1?.Apellidos;
            IdResponsable2 = compromiso.Responsable2?.IdUsuario;
            NomResponsable2 = compromiso.Responsable2?.Nombres + " " + compromiso.Responsable2?.Apellidos;
            Frecuencia_reporte = compromiso.Frecuencia_reporte;
            Sustentos= compromiso.Sustentos;
            Estado = compromiso.Estado;
            Aprueba = compromiso.Aprueba.IdUsuario;
            NomAprueba = compromiso.Aprueba?.Nombres + " " + compromiso.Aprueba?.Apellidos;
            Medio_verificacion = compromiso.Medio_verificacion;

            Ano = compromiso.Ano;
            Duplicado= compromiso.Duplicado;
            Id_TipoCompromiso=compromiso.TipoCompromiso?.Id;
            Nomb_TipoCompromiso = compromiso.TipoCompromiso?.Tipo;
            Actividad= compromiso.Actividad;
            Id_ComponenteAux= compromiso.ComponenteAux?.Id;
            Nomb_ComponenteAux = compromiso.ComponenteAux?.Componente;
            Id_FactorAmbiental=compromiso.FactorAmbiental?.Id;
            Nomb_FactorAmbiental = compromiso.FactorAmbiental?.Factor;
            Vigencia = compromiso.Vigencia;
            Motivo=compromiso.Motivo;
            Comentario= compromiso.Comentario;
        }
    }
}
