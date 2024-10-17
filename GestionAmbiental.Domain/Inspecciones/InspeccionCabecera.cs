using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionCabecera
    {
        public int IdCabecera { get; set; }
        public string Codigo { get; set; }
        public string Codigo_Formato { get; set; }
        public string Area_Formato { get; set; }
        public int? Id_Miembro_Comite { get; set; }
        public string Miembro_Comite { get; set; }
        public string Participantes_Inspección { get; set; }
        public List<Usuario> Participantes_Inspección_Detalle { get; set; }
        public int IdRegistradoPor {get;set;}
        public string RegistradoPor {get;set;}
        public string Objetivo { get; set; }
        public int IdGerencia { get; set; }
        public string Gerencia { get; set; }
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string TipoInspeccion { get; set; }
        public int? IdGerenciaInspeccionada { get; set; }
        public string GerenciaInspeccionada { get; set; }
        public string AreaInspeccionada { get; set; }
        public int IdResponsableArea { get; set; }
        public Usuario ResponsableArea { get; set; }
        public DateTime FechaInspeccion { get; set; }
        public int NObservaciones { get; set; }
        public int NObsImplementadas { get; set; }
        public int NObsAprobadas { get; set; }
        public int NObsRechazadas { get; set; }
        public string ResponsableInspeccion { get; set; }
        public string Participantes { get; set; }
        public string Estado { get; set; }
        public List<InspeccionDetalle> Observaciones { get; set; }
        public bool Permiso { get; set; }
        public bool PermisoAdmin { get; set; }

        public InspeccionCabecera(
            int idCabecera, string codigo, string gerenEjecMar,
            string empContratMarc, string lugarInspec, DateTime fechaInspec,
            int nObservaciones, string estado,string gerenciaInspec)
        {
            IdCabecera = idCabecera;
            Codigo = codigo;
            Gerencia = gerenEjecMar;
            Empresa = empContratMarc;
            AreaInspeccionada = lugarInspec;
            GerenciaInspeccionada = gerenciaInspec;
            FechaInspeccion = fechaInspec;
            NObservaciones = nObservaciones;
            Estado = estado;
        }

        public InspeccionCabecera(
            int idCabecera, 
            string codigo, 
            string objetivo,
            string gerenEjecMar, 
            string empContratMarc,
            string lugarInspec,
            Usuario responsable_Lugar,
            DateTime fechaInspec, 
            string inspectores,
            string estado)
        {
            IdCabecera = idCabecera;
            Codigo = codigo;
            Objetivo = objetivo;
            Gerencia = gerenEjecMar;
            Empresa = empContratMarc;
            AreaInspeccionada = lugarInspec;
            ResponsableArea = responsable_Lugar;
            FechaInspeccion = fechaInspec;
            Participantes = inspectores;
            Estado = estado;
        }
        public InspeccionCabecera()
        {
        }
    }
}
