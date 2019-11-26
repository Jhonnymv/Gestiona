using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Gerencia
    {
        public int Id_Gerencia { get; set; }
        public string Descripcion { get; set; }

        public Gerencia(int id_Gerencia, string descripcion)
        {
            Id_Gerencia = id_Gerencia;
            Descripcion = descripcion;
        }
    }
}
