using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.CompromisosMCB
{
    [Table("Tb_Compromisos_CMCB")]
    public class CompromisoCMCB
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName ="Varchar(255)")]
        public string Codigo { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Criterio { get; set; }

        [Column(TypeName = "Text")]
        public string Detalle_compromiso { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Etapa { get; set; }

        [Column(TypeName = "Text")]
        public string Texto_compromiso { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Origen { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string Tomo { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string Capitulo { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string Seccion { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string Pagina { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string Resolucion { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Autoridad { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Compromiso { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Aspecto { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Componente { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Gerencia { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Area { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Frecuencia { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Entidad { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable1 { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Responsable2 { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Frecuencia_reporte { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Sustentos { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Estado { get; set; }

        [Column(TypeName = "int")]
        public int? Id_Aprueba { get; set; }

        [Column(TypeName = "Varchar(255)")]
        public string? Medio_verificacion { get; set; }
    }
}
