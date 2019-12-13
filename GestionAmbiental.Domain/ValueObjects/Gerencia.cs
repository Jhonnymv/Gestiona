using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Gerencia
    {
        public int IdGerencia { get; set; }
        public string Descripcion { get; set; }

        public Gerencia()
        {
        }

        public Gerencia(int id_Gerencia, string descripcion)
        {
            IdGerencia = id_Gerencia;
            Descripcion = descripcion;
        }
    }
}
