using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Info_SGA
    {
        public int Id_Info { get; set; }
        public string Info { get; set; }
        public string Tipo { get; set; }
        public Info_SGA()
        {
        }
        public Info_SGA(int id_Info, string tipo, string info)
        {
            Id_Info = id_Info;
            Tipo = tipo;
            Info = info;
        }
    }
}
