using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;



namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Inspeccion_Cabecera")]
    public class Inspeccion_Cabecera
    {
        [Key]
        public int Id_Inspeccion_Cabecera { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Codigo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Registrado_Por { get; set; }
        [Column(TypeName = "int")]
        public int Id_Gerencia_Ejecutora { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa_Contratista { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Inspeccion { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Lugar_Inspeccionado { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Tipo_Inspeccion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Responsable_Lugar { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Responsable_Inspeccion { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Inspectores { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Objetivo { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
