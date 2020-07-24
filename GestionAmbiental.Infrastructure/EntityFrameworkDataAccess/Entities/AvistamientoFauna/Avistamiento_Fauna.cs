using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Avistamiento_Fauna")]
    public class Avistamiento_Fauna
    {
        [Key]
        public int Id_Avistamiento_Fauna { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Codigo { get; set; }
        [Column(TypeName = "text")]
        public string Fotos { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Avistamiento { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Ubicacion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Nombre_Comun_Fauna { get; set; }
        [Column(TypeName = "int")]
        public int Cantidad { get; set; }
        [Column(TypeName = "int")]
        public int Id_Tipo_Avistamiento { get; set; }
        [Column(TypeName = "int")]
        public int Id_Estado_Fauna { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Lugar_Referencia { get; set; }
        [Column(TypeName = "int")]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "int")]
        public int Id_Empresa { get; set; }
        [Column(TypeName = "int")]
        public int Id_Area_Trabajo { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string N_Fotocheck { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Descripcion_Accion { get; set; }
        [Column(TypeName = "text")]
        public string Evidencia_Accion { get; set; }
        [Column(TypeName = "int")]
        public int Id_Supervisor { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Estado { get; set; }
    }
}
