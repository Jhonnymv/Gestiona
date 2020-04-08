using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.Items
{
    public class SelectItemsOutput
    {
        public List<Componente_Ambiental> ItemsCompAmb { get; set; }
        public List<Especie_Fauna> ItemsEspFauna { get; set; }
        public List<Especie_Flora> ItemsEspFlora { get; set; }
        public List<Estado_Equipo_Incidente> ItemsEstadoEquipo { get; set; }
        public List<Lugar_Disposicion> ItemsLugarDiposicion { get; set; }
        public List<Nivel_Incidente> ItemsNivelIncidente { get; set; }
        public List<Producto_Incidente> ItemsProductoIncidente { get; set; }
        public List<Tipo_Agua> ItemsTipoAgua { get; set; }
        public List<Tipo_Incidente> ItemsTipoIncidente { get; set; }
        public List<Tipo_Producto_Incidente> ItemsTipoProducto { get; set; }
        public List<Tipo_Suelo> ItemsTipoSuelo { get; set; }
        public List<Unidad_Medida> ItemsUnidadMedida { get; set; }
        public List<TipoCondicion> ItemsTipoCondicion { get; set; }
        public List<Gerencia> ItemsGerencia { get; set; }
        public List<Usuario> ItemsUsuarioMA { get; set; }
        public List<Usuario> ItemsUsuario { get; set; }
    }
}
