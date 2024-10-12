using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Tipo_Inspeccion
    {
        public int Id_Tipo_Inspeccion { get; set; }
        public string Descripcion { get; set; }

        public Tipo_Inspeccion()
        {
        }
    }
}
