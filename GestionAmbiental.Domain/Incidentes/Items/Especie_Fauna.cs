using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Especie_Fauna
    {
        public int? Id_Especie_Fauna { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
