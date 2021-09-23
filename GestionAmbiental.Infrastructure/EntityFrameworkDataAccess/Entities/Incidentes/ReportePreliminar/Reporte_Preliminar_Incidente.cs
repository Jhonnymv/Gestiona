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
        [Column(TypeName = "varchar(10)")]
        public string Codigo { get; set; }
        [Column(TypeName = "int")]
        public int Num_Trabajadores { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Razon_Social_Contratista { get; set; }
        [Column(TypeName = "varchar(11)")]
        public string RUC_Contratista { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Actividad_Contratista { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Domicilio_Contratista { get; set; }
        [Column(TypeName = "int")]
        public int? Num_Trabajadores_Contratista { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Tipo_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Nivel_Incidente { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Incidente { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Producto_Derramado { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Cantidad_Derramado { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Cantidad_Derramado { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Area_Afectada { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Area_Afectada { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Lugar_Incidente { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Equipo_Involucrado { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Componente_Ambiental { get; set; }
        [Column(TypeName = "varchar(1500)")]
        public string Descripcion_Incidente { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Otra_Causa_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Gerencia_Responsable { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Comentario_Accion { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Supervisor_MA { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Estado { get; set; }
        [Column(TypeName = "text")]
        public string Nombres_Archivos { get; set; }
    }
}
