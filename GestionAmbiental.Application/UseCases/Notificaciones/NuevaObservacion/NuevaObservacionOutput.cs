using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion
{
    public class NuevaObservacionOutput
    {
        public int IdInspeccionCabecera { get; set; }
        public int IdInspeccionDetalle { get; set; }
        public string Codigo { get; set; }
        public string AreaEjecutora { get; set; }
        public string Empresa { get; set; }
        public string GerenciaInspeccionada { get; set; }
        public string AreaInspeccionada { get; set; }
        public string TipoInspeccion { get; set; }
        public string FechaInspeccion { get; set; }
        public string ResponsableLugar { get; set; }
        public string ResponsableInspeccion { get; set; }
        public string Participantes { get; set; }
        public string Objetivo { get; set; }
        public string TipoObservacion { get; set; }
        public string Asesor { get; set; }
        public string DescripcionCondicion { get; set; }
        public string TipoCondicion { get; set; }
        public string DescripcionActoCondicion { get; set; }
        public List<string> EvidenciaCondicion { get; set; }
        public string NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public string ResponsableMarcobre { get; set; }
        public string EmailResponsableMarcobre { get; set; }
        public string ResponsableContratista { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public string Estado { get; set; }
    }
}
