using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.Inspecciones
{
    public class FiltroReportesInspeccionesModel
    {
        //Inspecciones
        [DisplayName("Área Ejecutora de la Inspección:")]
        public int IdGerencia { get; set; }
        [DisplayName("Empresa Inspeccionada:")]
        public int IdEmpresa { get; set; }
        [DisplayName("Fecha de Inicio:")]
        public string FInicio { get; set; }
        [DisplayName("Fecha de Fin:")]
        public string FFin { get; set; }
        //Observaciones
        [DisplayName("Asesor MA/SSO:")]
        public int IdAsesor { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.:")]
        public int IdRespLev { get; set; }
    }
}
