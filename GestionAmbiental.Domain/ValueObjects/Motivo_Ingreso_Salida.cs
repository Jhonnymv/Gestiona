using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Motivo_Ingreso_Salida
    {
        public int Id_Motivo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public Motivo_Ingreso_Salida()
        {
        }

        public Motivo_Ingreso_Salida(int id_Motivo, string descripcion, string estado)
        {
            Id_Motivo = id_Motivo;
            Descripcion = descripcion;
            Estado = estado;
        }
    }
}
