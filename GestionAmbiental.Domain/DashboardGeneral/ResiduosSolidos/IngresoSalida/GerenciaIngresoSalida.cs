using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida
{
    public class GerenciaIngresoSalida
    {
        public string Gerencia { get; set; }
        public double Ingreso {get;set;}
        public double Salida {get;set;}
        public double Total {get;set;}
    }
}