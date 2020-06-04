using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente
{
    public class EstadosEquiposOutput
    {
        public List<EstadoEquipoOutput> Estados { get; set; }
        public int NEstados { get; set; }
    }
}
