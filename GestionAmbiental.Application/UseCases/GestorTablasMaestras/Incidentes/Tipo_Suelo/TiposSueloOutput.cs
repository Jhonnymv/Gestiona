using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo
{
    public class TiposSueloOutput
    {
        public List<TipoSueloOutput> Tipos { get; set; }
        public int NTipos { get; set; }
    }
}
