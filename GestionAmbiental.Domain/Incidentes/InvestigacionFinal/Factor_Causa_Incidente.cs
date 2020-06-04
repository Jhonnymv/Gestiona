using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Factor_Causa_Incidente
    {
        public int Id_Factor_Causa_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Tipo_Factor { get; set; }
        public string Estado { get; set; }

        public Factor_Causa_Incidente()
        {
        }

        public Factor_Causa_Incidente(int id_Factor_Causa_Incidente, string descripcion, string tipo_Factor, string estado)
        {
            Id_Factor_Causa_Incidente = id_Factor_Causa_Incidente;
            Descripcion = descripcion;
            Tipo_Factor = tipo_Factor;
            Estado = estado;
        }
    }
}
