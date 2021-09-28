using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.DashboardGeneral.Inspecciones
{
    public class InspeccionesMAPSSOOutput
    {
        public List<Dimension> ResultadosMAP { get; set; }
        public List<Dimension> ResultadosSSO { get; set; }
        public List<Dimension> ResultadosTodos { get; set; }
    }
}
