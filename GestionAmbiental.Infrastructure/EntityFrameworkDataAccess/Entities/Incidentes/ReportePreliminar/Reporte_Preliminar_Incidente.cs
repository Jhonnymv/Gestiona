using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.ReportePreliminar
{
    [Table("Tb_Reporte_Preliminar_Incidente")]
    public class Reporte_Preliminar_Incidente
    {
        [Key]
        public int Id_Reporte_Preliminar { get; set; }
        public int Num_Trabajadores { get; set; }
    }
}
