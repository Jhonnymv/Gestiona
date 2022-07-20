using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida
{
    public class IngresoSalidaNombreResiduo
    {
        public string NombreResiduo {get;set;}
        public List<DestinoResiduos> Destinos {get;set;}
        public List<GerenciaIngresoSalida> Gerencias {get;set;}
        public double TotalIngreso {get;set;}
        public double TotalSalida {get;set;}
        public double Total {get;set;}

        public IngresoSalidaNombreResiduo(){
            Destinos = new List<DestinoResiduos>();
            Gerencias = new List<GerenciaIngresoSalida>();
        }
    }
}