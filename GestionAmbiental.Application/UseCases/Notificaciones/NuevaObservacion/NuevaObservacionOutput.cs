using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion
{
    public class NuevaObservacionOutput
    {
        /*
             Área Ejecutora{0}
             Empresa{1}
             Área Inspeccionada{2}
             Tipo Inspección{3}
             Fecha Inspección{4}
             Responsable Lugar Inspeccionado{5}
             Responsable de la inspección{6}
             Participantes {7}
             Objetivo de la Inspección{8}

             TipoInspeccion{9}
             Asesor SSO/MAP{10}
             Acto / Condición Subestándar Identificada{11}
             Tipo de Condición / Acto Subestándar{12}
             Nivel de Riesgo del Aspecto Ambiental{13}
             Acción Preventiva / Correctiva Tomada{14}
             Responsable Marcobre{15}
             Responsable Contratista{16}
             Fecha Programada{17}
             */
        public int IdInspeccionCabecera { get; set; }
        public int IdInspeccionDetalle { get; set; }
        public string Codigo { get; set; }
        public string AreaEjecutora { get; set; }
        public string Empresa { get; set; }
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
