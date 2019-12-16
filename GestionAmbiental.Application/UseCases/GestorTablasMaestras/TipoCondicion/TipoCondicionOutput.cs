using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion
{
    public class TipoCondicionOutput
    {
        public int IdTipoCondicion { get; set; }
        public string Descripcion { get; set; }
        public string Grupo { get; set; }

        public TipoCondicionOutput()
        {
        }

        public TipoCondicionOutput(GestionAmbiental.Domain.ValueObjects.TipoCondicion tipoCondicion)
        {
            IdTipoCondicion = tipoCondicion.IdTipoCondicion;
            Descripcion = tipoCondicion.Descripcion;
            Grupo = tipoCondicion.Grupo;
        }
    }
}
