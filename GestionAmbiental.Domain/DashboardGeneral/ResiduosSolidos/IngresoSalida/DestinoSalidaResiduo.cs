using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida
{
    public class DestinoResiduos
    {
        public string Destino {get;set;}
        public List<NombreResiduoxIngresoSalida> NombreResiduos{get;set;}
        public double TotalIngreso {get;set;}
        public double TotalSalida {get;set;}
        public double Total {get;set;}

        public DestinoResiduos(){
            NombreResiduos = new List<NombreResiduoxIngresoSalida>();
        }
    }
}