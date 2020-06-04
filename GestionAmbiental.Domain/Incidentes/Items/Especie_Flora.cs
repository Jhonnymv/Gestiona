using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Especie_Flora
    {
        public int? Id_Especie_Flora { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Especie_Flora()
        {
        }

        public Especie_Flora(int? id_Especie_Flora, string descripcion, string estado)
        {
            Id_Especie_Flora = id_Especie_Flora;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
