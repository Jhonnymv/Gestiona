using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Especie_Flora
    {
        public int Id_Especie_Flora { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
