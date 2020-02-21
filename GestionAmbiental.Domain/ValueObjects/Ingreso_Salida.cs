using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Ingreso_Salida
    {
        public int Id_Ingreso_Salida { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Ingreso_Salida()
        {
        }

        public Ingreso_Salida(int id_Ingreso_Salida, string descripcion, string estado)
        {
            Id_Ingreso_Salida = id_Ingreso_Salida;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
