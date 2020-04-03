using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    [Table("Tb_Impacto_Ambiental_Incidente")]
    public class Impacto_Ambiental_Incidente
    {
        public int Id_Impacto_Ambiental_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        //-Suelo
        public int? Id_Tipo_Suelo { get; set; }
        public double? Area_Suelo { get; set; }
        public int? Id_Unidad_Area_Suelo { get; set; }
        public double? Cantidad_Suelo { get; set; }
        public int? Id_Unidad_Cantidad_Suelo { get; set; }
        public int? Id_Lugar_Disposicion_Suelo { get; set; }
        //-Flora
        public int? Id_Especie_Flora { get; set; }
        public double? Unidad_Flora { get; set; }
        public int? Id_Unidad_Flora { get; set; }
        public double? Cantidad_Residuo_Flora { get; set; }
        public int? Id_Unidad_Residuo_Flora { get; set; }
        public int? Id_Lugar_Disposicion_Flora { get; set; }
        //-Fauna
        public int? Id_Especie_Fauna { get; set; }
        public double? Unidad_Fauna { get; set; }
        public int? Id_Unidad_Fauna { get; set; }
        public double? Cantidad_Residuo_Fauna { get; set; }
        public int? Id_Unidad_Residuo_Fauna { get; set; }
        public int? Id_Lugar_Disposicion_Fauna { get; set; }
        //-Agua
        public int? Id_Tipo_Agua { get; set; }
        public double? Volumen_Agua { get; set; }
        public int? Id_Unidad_Agua { get; set; }
        public double? Cantidad_Residuo_Agua { get; set; }
        public int? Id_Unidad_Residuo_Agua { get; set; }
        public int? Id_Lugar_Disposicion_Agua { get; set; }

        public string Restos_Arq { get; set; }
        public string Nombre_Resto_Arq { get; set; }
        public string Cord_Este { get; set; }
        public string Cord_Norte { get; set; }
        public string Danio_Salud { get; set; }
        public int? Personas_Afectadas { get; set; }
        public double? Cantidad_RS { get; set; }
        public int? Id_Unidad_Cantidad_RS { get; set; }
        public double? Volumen_Rs { get; set; }
        public int? Id_Unidad_Volumen_RS { get; set; }
        public string EO_RS { get; set; }
    }
}
