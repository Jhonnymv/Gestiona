using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class NuevaObservacionModel
    {
        [DisplayName("Acto/Condición Subestándar Identificada")]
        public string CondicionIdentificada { get; set; }
        [DisplayName("Tipo de Condición/Acto Subestándar")]
        public int TipoCondicion { get; set; }
        [DisplayName("Evidencia")]
        public List<IFormFile> Evidencias { get; set; }
        [DisplayName("Nivel de Riesgo del Aspecto Ambiental")]
        public string NiveRiesgo { get; set; }
        [DisplayName("Acción Preventiva / Correctiva Tomada")]
        public string AccionTomada { get; set; }
        [DisplayName("Responsable Marcobre")]
        public int ResponsableMarc { get; set; }
        [DisplayName("Responsable Contratista")]
        public string ResponsableCont { get; set; }
        [DisplayName("Fecha Programada")]
        public string FechaProgramada { get; set; }

        public NuevaObservacionModel()
        {
        }
    }
}
