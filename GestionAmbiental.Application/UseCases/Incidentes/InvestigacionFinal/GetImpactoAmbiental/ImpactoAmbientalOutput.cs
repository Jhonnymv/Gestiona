using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetImpactoAmbiental
{
    public class ImpactoAmbientalOutput
    {
        public int Id_Impacto_Ambiental_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        //-Suelo
        public int? Id_Tipo_Suelo { get; set; }
        public string Tipo_Suelo { get; set; }
        public double? Area_Suelo { get; set; }
        public int? Id_Unidad_Area_Suelo { get; set; }
        public string Unidad_Area_Suelo { get; set; }
        public double? Cantidad_Suelo { get; set; }
        public int? Id_Unidad_Cantidad_Suelo { get; set; }
        public string Unidad_Cantidad_Suelo { get; set; }
        public int? Id_Lugar_Disposicion_Suelo { get; set; }
        public string Lugar_Disposicion_Suelo { get; set; }
        //-Flora
        public int? Id_Especie_Flora { get; set; }
        public string Especie_Flora { get; set; }
        public double? Unidad_Flora { get; set; }
        public int? Id_Unidad_Flora { get; set; }
        public string UnidadM_Flora { get; set; }
        public double? Cantidad_Residuo_Flora { get; set; }
        public int? Id_Unidad_Residuo_Flora { get; set; }
        public string Unidad_Residuo_Flora { get; set; }
        public int? Id_Lugar_Disposicion_Flora { get; set; }
        public string Lugar_Disposicion_Flora { get; set; }
        //-Fauna
        public int? Id_Especie_Fauna { get; set; }
        public string Especie_Fauna { get; set; }
        public double? Unidad_Fauna { get; set; }
        public int? Id_Unidad_Fauna { get; set; }
        public string UnidadM_Fauna { get; set; }
        public double? Cantidad_Residuo_Fauna { get; set; }
        public int? Id_Unidad_Residuo_Fauna { get; set; }
        public string Unidad_Residuo_Fauna { get; set; }
        public int? Id_Lugar_Disposicion_Fauna { get; set; }
        public string Lugar_Disposicion_Fauna { get; set; }
        //-Agua
        public int? Id_Tipo_Agua { get; set; }
        public string Tipo_Agua { get; set; }
        public double? Volumen_Agua { get; set; }
        public int? Id_Unidad_Agua { get; set; }
        public string UnidadM_Agua { get; set; }
        public double? Cantidad_Residuo_Agua { get; set; }
        public int? Id_Unidad_Residuo_Agua { get; set; }
        public string Unidad_Residuo_Agua { get; set; }
        public int? Id_Lugar_Disposicion_Agua { get; set; }
        public string Lugar_Disposicion_Agua { get; set; }

        public string Restos_Arq { get; set; }
        public string Nombre_Resto_Arq { get; set; }
        public string Cord_Este { get; set; }
        public string Cord_Norte { get; set; }
        public string Danio_Salud { get; set; }
        public int? Personas_Afectadas { get; set; }
        public double? Cantidad_RS { get; set; }
        public int? Id_Unidad_Cantidad_RS { get; set; }
        public string Unidad_Cantidad_RS { get; set; }
        public double? Volumen_Rs { get; set; }
        public int? Id_Unidad_Volumen_RS { get; set; }
        public string Unidad_Volumen_RS { get; set; }
        public string EO_RS { get; set; }

        public ImpactoAmbientalOutput(Domain.Incidentes.InvestigacionFinal.Impacto_Ambiental_Incidente impacto)
        {
            Id_Impacto_Ambiental_Incidente = impacto.Id_Impacto_Ambiental_Incidente;
            Id_Investigacion_Final = impacto.Id_Investigacion_Final;

            //Suelo
            Id_Tipo_Suelo = impacto.Tipo_Suelo?.Id_Tipo_Suelo;
            Tipo_Suelo = impacto.Tipo_Suelo?.Descripcion;

            Area_Suelo = impacto.Area_Suelo;

            Id_Unidad_Area_Suelo = impacto.Unidad_Area_Suelo?.Id_Unidad_Medida;
            Unidad_Area_Suelo = impacto.Unidad_Area_Suelo?.Descripcion;

            Cantidad_Suelo = impacto.Cantidad_Suelo;

            Id_Unidad_Cantidad_Suelo = impacto.Unidad_Cantidad_Suelo?.Id_Unidad_Medida;
            Unidad_Cantidad_Suelo = impacto.Unidad_Cantidad_Suelo?.Descripcion;

            Id_Lugar_Disposicion_Suelo = impacto.Lugar_Disposicion_Suelo?.Id_Lugar_Disposicion;
            Lugar_Disposicion_Suelo = impacto.Lugar_Disposicion_Suelo?.Descripcion;

            //Flora
            Id_Especie_Flora = impacto.Especie_Flora?.Id_Especie_Flora;
            Especie_Flora = impacto.Especie_Flora?.Descripcion;

            Unidad_Flora = impacto.Unidad_Flora;

            Id_Unidad_Flora = impacto.UnidadM_Flora?.Id_Unidad_Medida;
            UnidadM_Flora = impacto.UnidadM_Flora?.Descripcion;

            Cantidad_Residuo_Flora = impacto.Cantidad_Residuo_Flora;

            Id_Unidad_Residuo_Flora = impacto.Unidad_Residuo_Flora?.Id_Unidad_Medida;
            Unidad_Residuo_Flora = impacto.Unidad_Residuo_Flora?.Descripcion;

            Id_Lugar_Disposicion_Flora = impacto.Lugar_Disposicion_Flora?.Id_Lugar_Disposicion;
            Lugar_Disposicion_Flora = impacto.Lugar_Disposicion_Flora?.Descripcion;

            //Fauna
            Id_Especie_Fauna = impacto.Especie_Fauna?.Id_Especie_Fauna;
            Especie_Fauna = impacto.Especie_Fauna?.Descripcion;

            Unidad_Fauna = impacto.Unidad_Fauna;

            Id_Unidad_Fauna = impacto.UnidadM_Fauna?.Id_Unidad_Medida;
            UnidadM_Fauna = impacto.UnidadM_Fauna?.Descripcion;

            Cantidad_Residuo_Fauna = impacto.Cantidad_Residuo_Fauna;

            Id_Unidad_Residuo_Fauna = impacto.Unidad_Residuo_Fauna?.Id_Unidad_Medida;
            Unidad_Residuo_Fauna = impacto.Unidad_Residuo_Fauna?.Descripcion;

            Id_Lugar_Disposicion_Fauna = impacto.Lugar_Disposicion_Fauna?.Id_Lugar_Disposicion;
            Lugar_Disposicion_Fauna = impacto.Lugar_Disposicion_Fauna?.Descripcion;

            //Agua
            Id_Tipo_Agua = impacto.Tipo_Agua?.Id_Tipo_Agua;
            Tipo_Agua = impacto.Tipo_Agua?.Descripcion;

            Volumen_Agua = impacto.Volumen_Agua;

            Id_Unidad_Agua = impacto.Unidad_Agua?.Id_Unidad_Medida;
            UnidadM_Agua = impacto.Unidad_Agua?.Descripcion;

            Cantidad_Residuo_Agua = impacto.Cantidad_Residuo_Agua;
                        
            Id_Unidad_Residuo_Agua = impacto.Unidad_Residuo_Agua?.Id_Unidad_Medida;
            Unidad_Residuo_Agua = impacto.Unidad_Residuo_Agua?.Descripcion;

            Id_Lugar_Disposicion_Agua = impacto.Lugar_Disposicion_Agua?.Id_Lugar_Disposicion;
            Lugar_Disposicion_Agua = impacto.Lugar_Disposicion_Agua?.Descripcion;

            Restos_Arq = impacto.Restos_Arq;
            Nombre_Resto_Arq = impacto.Nombre_Resto_Arq;
            Cord_Este = impacto.Cord_Este;
            Cord_Norte = impacto.Cord_Norte;
            Danio_Salud = impacto.Danio_Salud;
            Personas_Afectadas = impacto.Personas_Afectadas;
            Cantidad_RS = impacto.Cantidad_RS;
            Id_Unidad_Cantidad_RS = impacto.Unidad_Cantidad_RS?.Id_Unidad_Medida;
            Unidad_Cantidad_RS = impacto.Unidad_Cantidad_RS?.Descripcion;
            Volumen_Rs = impacto.Volumen_Rs;
            Id_Unidad_Volumen_RS = impacto.Unidad_Volumen_RS?.Id_Unidad_Medida;
            Unidad_Volumen_RS = impacto.Unidad_Volumen_RS?.Descripcion;

            EO_RS = impacto.EO_RS;
        }
    }
}
