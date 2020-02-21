using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Tipo_Residuo
    {
        public int Id_Tipo_Residuo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Tipo_Residuo()
        {
        }

        public Tipo_Residuo(int id_Tipo_Residuo, string descripcion, string estado)
        {
            Id_Tipo_Residuo = id_Tipo_Residuo;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
