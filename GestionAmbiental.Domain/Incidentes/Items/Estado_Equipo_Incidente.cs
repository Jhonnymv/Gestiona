using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Estado_Equipo_Incidente
    {
        public int Id_Estado_Equipo_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
