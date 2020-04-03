using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Causa_Incidente
    {
        public int Id_Causa_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public int? Id_Acto_Condicion { get; set; }
        public int? Id_Factor_Causa_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
