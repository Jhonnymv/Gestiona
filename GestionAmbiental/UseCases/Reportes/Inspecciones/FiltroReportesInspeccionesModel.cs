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
        [DisplayName("Gerencia Ejecutora de la Inspección:")]
        public int IdGerencia { get; set; }
        [DisplayName("Empresa Inspeccionada:")]
        public int IdEmpresa { get; set; }
        [DisplayName("Desde:")]
        public string FInicio { get; set; }
        [DisplayName("Hasta:")]
        public string FFin { get; set; }
        //Observaciones
        [DisplayName("Asesor MA/SSO:")]
        public int IdAsesor { get; set; }
        [DisplayName("Responsable de Marcobre para levantar Obs.:")]
        public int IdRespLev { get; set; }
        [DisplayName("Gerencia Inspeccionada:")]
        public int IdGerenciaInspeccionada { get; set; }
        [DisplayName("Responsable de la Inspección o Participantes de la Inspección:")]
        public int UsuarioInvolucrado { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
        [DisplayName("Tipo Observación:")]
        public string TipoObservacion { get; set; }
        [DisplayName("¿Inluir Evidencias?")]
        public bool IncluirEvidencias { get; set; }
    }
}
