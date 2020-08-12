using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna
{
    public class EstadosFaunaOutput
    {
        public List<EstadoFaunaOutput> EstadosFauna { get; set; }
        public int NEstadosFauna { get; set; }
    }
}
