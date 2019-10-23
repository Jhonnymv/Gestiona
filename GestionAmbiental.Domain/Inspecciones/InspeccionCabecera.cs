using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionCabecera
    {
        public int IdCabecera { get; set; }
        public string Codigo { get; set; }
        public string GerenEjecMar { get; set; }
        public string EmpContratMarc { get; set; }
        public string GerenEjecEPCM { get; set; }
        public string EmpContratEPCM { get; set; }
        public string LugarInspec { get; set; }
        public DateTime FechaInspec { get; set; }
        public int NObservaciones { get; set; }
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
    }
}
