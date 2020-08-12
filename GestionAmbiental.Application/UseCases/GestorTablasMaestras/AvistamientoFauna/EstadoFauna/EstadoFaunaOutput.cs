using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna
{
    public class EstadoFaunaOutput
    {
        public int IdEstadoFauna { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public EstadoFaunaOutput()
        {
        }
        public EstadoFaunaOutput(Domain.AvistamientoFauna.Items.EstadoFauna estadoFauna)
        {
            IdEstadoFauna = estadoFauna.IdEstadoFauna;
            Descripcion = estadoFauna.Descripcion;
            Estado = estadoFauna.Estado;
        }
    }
}
