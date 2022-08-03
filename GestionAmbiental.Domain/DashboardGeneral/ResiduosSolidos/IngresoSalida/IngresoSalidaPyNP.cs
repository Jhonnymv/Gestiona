using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida
{
    public class IngresoSalidaPyNP
    {
        public IngresoSalidaResiduo Peligrosos {get;set;}
        public IngresoSalidaResiduo NoPeligrosos {get;set;}
        public IngresoSalidaPyNP(){
            Peligrosos = new IngresoSalidaResiduo();
            NoPeligrosos = new IngresoSalidaResiduo();
        }
    }    
}