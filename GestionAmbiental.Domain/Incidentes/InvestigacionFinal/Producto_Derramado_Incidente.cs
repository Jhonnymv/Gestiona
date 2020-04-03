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
        public int? Id_Producto_Incidente { get; set; }
        public int? Id_Tipo_Producto { get; set; }
        public string Estado_Producto { get; set; }
        public decimal? Cantidad_Derramada { get; set; }
        public int? Id_Unidad { get; set; }
        public string Equipo_Responsable { get; set; }
        public int? Id_Estado_Equipo_Responsable { get; set; }
        public decimal? Cantidad_Recuperado { get; set; }
        public decimal? Cantidad_No_Recuperado { get; set; }
        public string Estado { get; set; }
    }
}
