using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Producto_Incidente
    {
        public int? Id_Producto_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Producto_Incidente()
        {
        }

        public Producto_Incidente(int? id_Producto_Incidente, string descripcion, string estado)
        {
            Id_Producto_Incidente = id_Producto_Incidente;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
