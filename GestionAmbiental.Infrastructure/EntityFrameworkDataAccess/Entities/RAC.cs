using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_RAC")]
    public class RAC
    {
        [Key]
        public int Id_RAC { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Codigo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres_Apellidos { get; set; }
        [Column(TypeName = "int")]
        public int Id_Registrado_Por { get; set; }
        [Column(TypeName = "int")]
        public int Id_Gerencia { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Otra_Empresa { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa_Obs { get; set; }
        [Column(TypeName = "int")]
        public int Id_Gerencia_Obs { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Ubicacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string SSO_MA { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Clasificacion { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string NivelRiesgo { get; set; }
        [Column(TypeName = "int")]
        public int Id_Tipo_Condicion { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Descripcion_Acto_Cond { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Medida_Control { get; set; }
        [Column(TypeName = "varchar(2)")]
        public string PAC { get; set; }
        [Column(TypeName = "int")]
        public int Id_Responsable_Cierre { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Cierre { get; set; }
        [Column(TypeName = "int")]
        public int Id_Supervisor { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia_Obs { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia_Cierre { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comentario_Cierre { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Comentario_Verificacion { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string Estado { get; set; }
    }
}
