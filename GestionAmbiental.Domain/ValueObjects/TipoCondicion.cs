using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class TipoCondicion
    {
        public int IdTipoCondicion { get; set; }
        public string Descripcion { get; set; }
        public string Grupo { get; set; }

        public TipoCondicion()
        {
        }

        public TipoCondicion(int id_Tipo_Condicion, string descipcion,string grupo)
        {
            IdTipoCondicion = id_Tipo_Condicion;
            Descripcion = descipcion;
            Grupo = grupo;
        }
    }
}
