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
        public int Id_Inspeccion_Cabecera { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Codigo { get; set; }
        public string Area_Ejecutora { get; set; }
        public string Empresa { get; set; }
        public string Area_Inspeccionada { get; set; }
        public string Tipo_Inspeccion { get; set; }
        public string Fecha_Inspeccion { get; set; }
        public string Responsable_Lugar { get; set; }
        public string ResponsableInspeccion { get; set; }
        public string Participantes { get; set; }
        public string Objetivo { get; set; }
        public string Tipo_Observacion { get; set; }
        public string Asesor_SSO_MAP { get; set; }
        public string Descripcion_Condicion { get; set; }
        public string Tipo_Condicion { get; set; }
        public List<string> Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Email_Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string Estado { get; set; }
    }
}
