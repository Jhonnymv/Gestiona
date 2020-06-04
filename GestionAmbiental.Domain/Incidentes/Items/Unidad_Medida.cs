using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Unidad_Medida
    {
        public int? Id_Unidad_Medida { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }

        public Unidad_Medida()
        {
        }

        public Unidad_Medida(int? id_Unidad_Medida, string descripcion, string tipo, string estado)
        {
            Id_Unidad_Medida = id_Unidad_Medida;
            Descripcion = descripcion;
            Tipo = tipo;
            Estado = estado;
        }
    }
}
