using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion
{
    public class NuevaObservacionOutput
    {
        /*
             Gerencia Inspeccionada Marcobre{0}
             Empresa Contratista Marcobre{1}
             Gerencia Inspeccionada EPCM{2}
             Empresa Contratista EPCM{3}
             Lugar Inspeccionado{4}
             Fecha Inspección{5}
             Responsable Lugar Inspeccionado{6}
             Responsable Medio Ambiente{7}
             Inspector(es){8}
             Objetivo de la Inspección{9}
             Acto / Condición Subestándar Identificada{10}
             Tipo de Condición / Acto Subestándar{11}
             Nivel de Riesgo del Aspecto Ambiental{12}
             Acción Preventiva / Correctiva Tomada{13}
             Responsable Marcobre{14}
             Responsable Contratista{15}
             Fecha Programada{16}
             */
        public int Id_Inspeccion_Cabecera { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Codigo { get; set; }
        public string Gerencia_Inspeccionada_Marcobre { get; set; }
        public string Empresa_Contratista_Marcobre { get; set; }
        public string Gerencia_Inspeccionada_EPCM { get; set; }
        public string Empresa_Contratista_EPCM { get; set; }
        public string Lugar_Inspeccionado { get; set; }
        public string Fecha_Inspeccion { get; set; }
        public string Responsable_Lugar { get; set; }
        public string Responsable_MA { get; set; }
        public string Inspectores { get; set; }
        public string Objetivo { get; set; }
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
