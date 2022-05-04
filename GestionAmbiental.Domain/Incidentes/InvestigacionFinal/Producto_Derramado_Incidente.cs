using GestionAmbiental.Domain.Incidentes.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Producto_Derramado_Incidente
    {
        public int Id_Producto_Derramado_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public Investigacion_Final_Incidente Investigacion_Final { get; set; }
        public Producto_Incidente Producto_Incidente { get; set; }
        public Tipo_Producto_Incidente Tipo_Producto { get; set; }
        public string Estado_Producto { get; set; }
        public double? Cantidad_Derramada { get; set; }
        public Unidad_Medida Unidad { get; set; }
        public string Equipo_Responsable { get; set; }
        public Estado_Equipo_Incidente Estado_Equipo_Responsable { get; set; }
        public double? Cantidad_Recuperado { get; set; }
        public double? Cantidad_No_Recuperado { get; set; }
        public string Estado { get; set; }
    }
}
