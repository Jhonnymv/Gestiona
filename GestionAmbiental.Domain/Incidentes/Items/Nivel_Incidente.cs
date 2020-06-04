using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Nivel_Incidente
    {
        public int? Id_Nivel_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Nivel_Incidente()
        {
        }

        public Nivel_Incidente(int? id_Nivel_Incidente, string descripcion, string estado)
        {
            Id_Nivel_Incidente = id_Nivel_Incidente;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
