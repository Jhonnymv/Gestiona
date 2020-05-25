using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos
{
    public class ProductoDerramadoOutput
    {
        public int Id_Producto_Derramado_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public int? Id_Producto_Incidente { get; set; }
        public string Producto_Incidente { get; set; }
        public int? Id_Tipo_Producto { get; set; }
        public string Tipo_Producto { get; set; }
        public string Estado_Producto { get; set; }
        public double? Cantidad_Derramada { get; set; }
        public int? Id_Unidad { get; set; }
        public string Unidad { get; set; }
        public string Equipo_Responsable { get; set; }
        public int? Id_Estado_Equipo_Responsable { get; set; }
        public string Estado_Equipo_Responsable { get; set; }
        public double? Cantidad_Recuperado { get; set; }
        public double? Cantidad_No_Recuperado { get; set; }
        public string Estado { get; set; }

        public ProductoDerramadoOutput(Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente producto)
        {
            Id_Producto_Derramado_Incidente = producto.Id_Producto_Derramado_Incidente;
            Id_Investigacion_Final = producto.Id_Investigacion_Final;

            Id_Producto_Incidente = producto.Producto_Incidente?.Id_Producto_Incidente;
            Producto_Incidente = producto.Producto_Incidente?.Descripcion;

            Id_Tipo_Producto = producto.Tipo_Producto?.Id_Tipo_Producto_Incidente;
            Tipo_Producto = producto.Tipo_Producto?.Descripcion;

            Estado_Producto = producto.Estado_Producto;
            Cantidad_Derramada = producto.Cantidad_Derramada;

            Id_Unidad = producto.Unidad?.Id_Unidad_Medida;
            Unidad = producto.Unidad?.Descripcion;

            Equipo_Responsable = producto.Equipo_Responsable;

            Id_Estado_Equipo_Responsable = producto.Estado_Equipo_Responsable?.Id_Estado_Equipo_Incidente;
            Estado_Equipo_Responsable = producto.Estado_Equipo_Responsable?.Descripcion;

            Cantidad_Recuperado = producto.Cantidad_Recuperado;
            Cantidad_No_Recuperado = producto.Cantidad_No_Recuperado;
            Estado = producto.Estado;
        }
    }
}
