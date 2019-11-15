using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Tipo_Condicion
    {
        public int Id_Tipo_Condicion { get; set; }
        public string Descripcion { get; set; }
        public string Grupo { get; set; }

        public Tipo_Condicion(int id_Tipo_Condicion, string descipcion,string grupo)
        {
            Id_Tipo_Condicion = id_Tipo_Condicion;
            Descripcion = descipcion;
            Grupo = grupo;
        }
    }
}
