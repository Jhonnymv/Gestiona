using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna
{
    public class TipoFaunaOutput
    {
        public int IdTipoFauna { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoFaunaOutput() { }
        public TipoFaunaOutput(Domain.AvistamientoFauna.Items.TipoFauna tipoFauna) {
            IdTipoFauna = tipoFauna.IdTipoFauna;
            Descripcion = tipoFauna.Descripcion;
            Estado = tipoFauna.Estado;

        }
    }
}
