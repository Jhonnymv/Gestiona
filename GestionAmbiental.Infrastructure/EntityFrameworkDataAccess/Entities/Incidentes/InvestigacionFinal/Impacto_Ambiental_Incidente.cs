using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Impacto_Ambiental_Incidente")]
    public class Impacto_Ambiental_Incidente
    {
        [Key]
        public int Id_Impacto_Ambiental_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        //-Suelo
        [Column(TypeName = "int")]
        public int? Id_Tipo_Suelo { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Area_Suelo { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Area_Suelo { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Cantidad_Suelo { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Cantidad_Suelo { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Lugar_Disposicion_Suelo { get; set; }
        //-Flora
        [Column(TypeName = "int")]
        public int? Id_Especie_Flora { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Unidad_Flora { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Flora { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Cantidad_Residuo_Flora { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Residuo_Flora { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Lugar_Disposicion_Flora { get; set; }
        //-Fauna
        [Column(TypeName = "int")]
        public int? Id_Especie_Fauna { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Unidad_Fauna { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Fauna { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Cantidad_Residuo_Fauna { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Residuo_Fauna { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Lugar_Disposicion_Fauna { get; set; }
        //-Agua
        [Column(TypeName = "int")]
        public int? Id_Tipo_Agua { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Volumen_Agua { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Agua { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Cantidad_Residuo_Agua { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Residuo_Agua { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Lugar_Disposicion_Agua { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string Restos_Arq { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Nombre_Resto_Arq { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Cord_Este { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Cord_Norte { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Danio_Salud { get; set; }
        [Column(TypeName = "int")]
        public int? Personas_Afectadas { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Cantidad_RS { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Cantidad_RS { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public double? Volumen_Rs { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Unidad_Volumen_RS { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string EO_RS { get; set; }
    }
}
