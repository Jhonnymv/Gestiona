using GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada
{
    public class ObservacionImplementadaOutput : NuevaObservacionOutput
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
            public string Fecha_Cierre { get; set; }
        public string Comentario_Implementacion { get; set; }
        public List<string> Evidencia_Implementacion { get; set; }
        public string Email_Asesor_MAP_SSO { get; set; }

    }
}
