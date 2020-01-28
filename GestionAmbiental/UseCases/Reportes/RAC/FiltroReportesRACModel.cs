using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.RAC
{
    public class FiltroReportesRACModel
    {
        [DisplayName("Gerencia Observada:")]
        public int IdGerencia { get; set; }
        [DisplayName("Empresa Observada:")]
        public int IdEmpresa { get; set; }
        [DisplayName("Usuario con Observación Asignada:")]
        public int IdUsuario { get; set; }
        [DisplayName("Fecha de Inicio:")]
        public string FInicio { get; set; }
        [DisplayName("Fecha de Fin:")]
        public string FFin { get; set; }
    }
}
