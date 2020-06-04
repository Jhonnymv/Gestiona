using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Tipo_Agua
    {
        public int? Id_Tipo_Agua { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Tipo_Agua()
        {
        }

        public Tipo_Agua(int? id_Tipo_Agua, string descripcion, string estado)
        {
            Id_Tipo_Agua = id_Tipo_Agua;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
