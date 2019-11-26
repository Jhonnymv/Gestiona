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
        public int Id_Registrado_Por {get;set;}
        public string Objetivo { get; set; }
        public int idGerenciaEjecMar { get; set; }
        public string GerenEjecMar { get; set; }
        public int idEmpContratMarc { get; set; }
        public string EmpContratMarc { get; set; }
        public string Tipo_Inspeccion { get; set; }
        public string LugarInspec { get; set; }
        public int idResponsableLugar { get; set; }
        public Usuario Responsable_Lugar { get; set; }
        public DateTime FechaInspec { get; set; }
        public int NObservaciones { get; set; }
        public int NObsImplementadas { get; set; }
        public int NObsAprobadas { get; set; }
        public int NObsRechazadas { get; set; }
        public string Responsable_Inspeccion { get; set; }
        public string Inspectores { get; set; }
        public string Estado { get; set; }
        public List<InspeccionDetalle> Observaciones { get; set; }

        public InspeccionCabecera(
            int idCabecera, string codigo, string gerenEjecMar,
            string empContratMarc, string lugarInspec, DateTime fechaInspec,
            int nObservaciones, string estado)
        {
            IdCabecera = idCabecera;
            Codigo = codigo;
            GerenEjecMar = gerenEjecMar;
            EmpContratMarc = empContratMarc;
            LugarInspec = lugarInspec;
            FechaInspec = fechaInspec;
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
            GerenEjecMar = gerenEjecMar;
            EmpContratMarc = empContratMarc;
            LugarInspec = lugarInspec;
            Responsable_Lugar = responsable_Lugar;
            FechaInspec = fechaInspec;
            Inspectores = inspectores;
            Estado = estado;
        }

        public InspeccionCabecera()
        {
        }
    }
}
