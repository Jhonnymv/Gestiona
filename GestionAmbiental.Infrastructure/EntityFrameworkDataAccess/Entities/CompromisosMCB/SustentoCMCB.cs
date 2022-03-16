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
        public DateTime? Fecha_limite { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? Fecha_entrega { get; set; }
    
        [Column(TypeName = "Date")]
        public DateTime? Fecha_validacion { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable1 { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable2 { get; set; }

        [Column(TypeName = "Int")]
        public int? Id_Asesor { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Estado { get; set; }

        [Column(TypeName = "Text")]
        public string? Sustento_cumplimiento{ get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Evidencia { get; set; }

        [Column(TypeName = "Text")]
        public string? Detalle_compromiso { get; set; }

        [Column(TypeName = "Text")]
        public string? Comentario_implementacion { get; set; }


        [Column(TypeName = "Text")]
        public string? Comentario_verificacion { get; set; }


        [Column(TypeName = "varchar(255)")]
        public string? Medio_verificacion { get; set; }

    }
}
