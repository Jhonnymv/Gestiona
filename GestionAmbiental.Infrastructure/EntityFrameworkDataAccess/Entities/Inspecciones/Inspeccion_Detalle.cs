using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Inspecciones
{
    [Table("Tb_Inspeccion_Detalle")]
    public class Inspeccion_Detalle
    {
        [Key]
        public int Id_Inspeccion_Detalle { get; set; }
        [Column(TypeName = "int")]
        public int Id_Inspeccion_Cabecera { get; set; }
        [Column(TypeName = "varchar(35)")]
        public string Tipo_Observacion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Asesor { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Descripcion_Condicion { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Tipo_Condicion { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia_Condicion { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Nivel_Riesgo { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Descripcion_Accion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Responsable_Marcobre { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Responsable_Marcobre2 { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Responsable_Contratista { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Correo_Contratista { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Programada { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia_Implementacion { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comentario_Implementacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Cierre { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Verificacion { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comentario_Verificacion { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Descripcion_Acto_Condicion { get; set; }

    }
}
