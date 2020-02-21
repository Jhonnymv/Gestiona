using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Residuo_Solido
    {
        public int Id_Residuo_Solido { get; set; }
        public Tipo_Residuo Tipo_Residuo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Residuo_Solido()
        {
        }

        public Residuo_Solido(int id_Residuo_Solido, Tipo_Residuo tipo_Residuo, string descripcion, string estado)
        {
            Id_Residuo_Solido = id_Residuo_Solido;
            Tipo_Residuo = tipo_Residuo;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
