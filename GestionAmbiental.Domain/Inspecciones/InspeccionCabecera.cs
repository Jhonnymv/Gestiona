using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionCabecera
    {
        public int IdCabecera { get; set; }
        public string Codigo { get; set; }
        public string Objetivo { get; set; }
        public int idGerenciaEjecMar { get; set; }
        public string GerenEjecMar { get; set; }
        public int idEmpContratMarc { get; set; }
        public string EmpContratMarc { get; set; }
        public int idGerencEjectEPCM { get; set; }
        public string GerenEjecEPCM { get; set; }
        public int idEmpContratEPCM { get; set; }
        public string EmpContratEPCM { get; set; }
        public string LugarInspec { get; set; }
        public int idResponsableLugar { get; set; }
        public int idResponsableMA { get; set; }
        public DateTime FechaInspec { get; set; }
        public int NObservaciones { get; set; }
        public string Inspectores { get; set; }
        public string Estado { get; set; }

        public InspeccionCabecera(
            int idCabecera, string codigo, string gerenEjecMar, 
            string empContratMarc, string gerenEjecEPCM, 
            string empContratEPCM, string lugarInspec, DateTime fechaInspec, 
            int nObservaciones, string estado)
        {
            IdCabecera = idCabecera;
            Codigo = codigo;
            GerenEjecMar = gerenEjecMar;
            EmpContratMarc = empContratMarc;
            GerenEjecEPCM = gerenEjecEPCM;
            EmpContratEPCM = empContratEPCM;
            LugarInspec = lugarInspec;
            FechaInspec = fechaInspec;
            NObservaciones = nObservaciones;
            Estado = estado;
        }

        public InspeccionCabecera(
            int idCabecera, string codigo, int idGerenciaEjecMar, int idEmpContratMarc, 
            int idGerencEjectEPCM, int idEmpContratEPCM, string lugarInspec, DateTime fechaInspec, 
            int nObservaciones, string estado,int idResponsableLug, int idResponsableMA,string objetivo,string inspectores)
        {
            IdCabecera = idCabecera;
            Codigo = codigo;
            this.idGerenciaEjecMar = idGerenciaEjecMar;
            this.idEmpContratMarc = idEmpContratMarc;
            this.idGerencEjectEPCM = idGerencEjectEPCM;
            this.idEmpContratEPCM = idEmpContratEPCM;
            LugarInspec = lugarInspec;
            FechaInspec = fechaInspec;
            NObservaciones = nObservaciones;
            Estado = estado;
            idResponsableLugar = idResponsableLug;
            this.idResponsableMA = idResponsableMA;
            Objetivo = objetivo;
            Inspectores = inspectores;
        }
    }
}
