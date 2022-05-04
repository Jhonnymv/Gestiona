using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.ReportePreliminar
{
    public class Causa_Incidente_Reporte
    {
        public int Id_Causa_Incidente_Reporte { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public Reporte_Preliminar_Incidente Reporte { get; set; }
        public int? Id_Acto_Condicion { get; set; }
        public TipoCondicion? TipoCondicion {get;set;}
        public string Tipo_Acto_Condicion { get; set; }
        public string Estado { get; set; }
    }
}
