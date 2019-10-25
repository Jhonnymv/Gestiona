using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class NuevaInspeccionModel
    {
        [DisplayName("Código")]
        public string Codigo { get; set; }
        [DisplayName("Área")]
        public string Area { get; set; }
        [DisplayName("Gerencia Ejecutora Marcobre")]
        public int Id_Gerencia_Ejecutora { get; set; }
        [DisplayName("Gerencia EPCM")]
        public int Id_Gerencia_EPCM { get; set; }
        [DisplayName("Empresa Contratista Marcobre")]
        public int Id_Empresa_Contratista { get; set; }
        [DisplayName("Empresa Contratista EPCM")]
        public int Id_Empresa_Contratista_EPCM { get; set; }
        [DisplayName("Fecha")]
        public string Fecha_Inspeccion { get; set; }
        [DisplayName("Hora")]
        public string Hora_Inspeccion { get; set; }
        [DisplayName("Lugar Inspeccionado")]
        public string Lugar_Inspeccionado { get; set; }
        [DisplayName("Responsable Lugar Inspeccionado")]
        public int Id_Responsable_Lugar { get; set; }
        [DisplayName("Responsable Medio Ambiente")]
        public int Id_Responsable_MA { get; set; }
        [DisplayName("Inspector(es)")]
        public string Inspectores { get; set; }
        [DisplayName("Objetivo de la Inspección")]
        public string Objetivo { get; set; }
        public NuevaObservacionModel Observacion { get; set; }
    }
}
