using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida
{
    public class IngresoSalidaResiduo
    {
        public List<Dimension> Ingreso {get;set;}
        public List<Dimension> Salida {get;set;}
        public IngresoSalidaResiduo(){
            Ingreso = new List<Dimension>();
            Salida = new List<Dimension>();
        }
    }
}