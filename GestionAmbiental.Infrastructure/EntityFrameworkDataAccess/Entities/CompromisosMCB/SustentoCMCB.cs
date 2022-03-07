using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Sustentos_CMCB")]
    public class SustentoCMCB
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="int")]
        public int? Id_Compromiso { get; set; }

        [Column(TypeName = "Date")]
        public int? Fecha_limite { get; set; }

        [Column(TypeName = "Date")]
        public int? Fecha_entrega { get; set; }
    
        [Column(TypeName = "Date")]
        public int? Fecha_validacion { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable1 { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable2 { get; set; }

        [Column(TypeName = "Int")]
        public int? Id_Asesor { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public int? Estado { get; set; }

        [Column(TypeName = "Text")]
        public int? Sustento_cumplimiento{ get; set; }

        [Column(TypeName = "Varchar(255)")]
        public int? Evidencia { get; set; }


        [Column(TypeName = "Text")]
        public int? Comentario_imcumplimiento { get; set; }


        [Column(TypeName = "Text")]
        public int? Comentario_verificacion { get; set; }


        [Column(TypeName = "varchar(255)")]
        public int? Medio_verificacion { get; set; }

    }
}
