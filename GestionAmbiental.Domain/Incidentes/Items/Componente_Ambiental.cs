using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Componente_Ambiental
    {
        public int? Id_Componente_Ambiental { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
