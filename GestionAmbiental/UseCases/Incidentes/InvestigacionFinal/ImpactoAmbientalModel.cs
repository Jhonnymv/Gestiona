using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinal
{
    public class ImpactoAmbientalModel
    {
        public int Id_Impacto_Ambiental_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        //-Suelo
        [DisplayName("Tipo de suelo:")]
        public int? Id_Tipo_Suelo { get; set; }
        [DisplayName("Área de suelo afectada:")]
        public double? Area_Suelo { get; set; }
        public int? Id_Unidad_Area_Suelo { get; set; }
        [DisplayName("Cantidad de suelo impactado:")]
        public double? Cantidad_Suelo { get; set; }
        public int? Id_Unidad_Cantidad_Suelo { get; set; }
        [DisplayName("Lugar de disposición::")]
        public int? Id_Lugar_Disposicion_Suelo { get; set; }
        //-Flora
        [DisplayName("Especie:")]
        public int? Id_Especie_Flora { get; set; }
        [DisplayName("Unidades de flora:")]
        public double? Unidad_Flora { get; set; }
        public int? Id_Unidad_Flora { get; set; }
        [DisplayName("Cantidad de residuos generados:")]
        public double? Cantidad_Residuo_Flora { get; set; }
        public int? Id_Unidad_Residuo_Flora { get; set; }
        [DisplayName("Lugar de disposición::")]
        public int? Id_Lugar_Disposicion_Flora { get; set; }
        //-Fauna
        [DisplayName("Especie:")]
        public int? Id_Especie_Fauna { get; set; }
        [DisplayName("Número de individuos afectados:")]
        public double? Unidad_Fauna { get; set; }
        public int? Id_Unidad_Fauna { get; set; }
        [DisplayName("Cantidad de residuos generados:")]
        public double? Cantidad_Residuo_Fauna { get; set; }
        public int? Id_Unidad_Residuo_Fauna { get; set; }
        [DisplayName("Lugar de disposición:")]
        public int? Id_Lugar_Disposicion_Fauna { get; set; }
        //-Agua
        [DisplayName("Tipo de agua:")]
        public int? Id_Tipo_Agua { get; set; }
        [DisplayName("Volumen de agua alterada:")]
        public double? Volumen_Agua { get; set; }
        public int? Id_Unidad_Agua { get; set; }
        [DisplayName("Cantidad de residuos generados:")]
        public double? Cantidad_Residuo_Agua { get; set; }
        public int? Id_Unidad_Residuo_Agua { get; set; }
        [DisplayName("Lugar de disposición:")]
        public int? Id_Lugar_Disposicion_Agua { get; set; }

        [DisplayName("¿Restos arqueológicos?")]
        public string Restos_Arq { get; set; }
        [DisplayName("Nombre del resto arqueológico:")]
        public string Nombre_Resto_Arq { get; set; }
        [DisplayName("Este:")]
        public string Cord_Este { get; set; }
        [DisplayName("Norte:")]
        public string Cord_Norte { get; set; }
        [DisplayName("Descripción del daño a la salud:")]
        public string Danio_Salud { get; set; }
        [DisplayName("Número de personas afectadas:")]
        public int? Personas_Afectadas { get; set; }
        [DisplayName("Cantidad de Residuos Sólidos Generados:")]
        public double? Cantidad_RS { get; set; }
        public int? Id_Unidad_Cantidad_RS { get; set; }
        [DisplayName("Cantidad de Residuos Líquidos Generados:")]
        public double? Volumen_Rs { get; set; }
        public int? Id_Unidad_Volumen_RS { get; set; }
        [DisplayName("EO-RS (recolección, transporte y disposición):")]
        public string EO_RS { get; set; }
    }
}
