using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento
{
    public class TipoAvistamientoOutput
    {
        public int IdTipoAvistamiento { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoAvistamientoOutput()
        {
        }
        public TipoAvistamientoOutput(Domain.AvistamientoFauna.Items.TipoAvistamiento tipoAvistamiento)
        {
            IdTipoAvistamiento = tipoAvistamiento.IdTipoAvistamiento;
            Descripcion = tipoAvistamiento.Descripcion;
            Estado = tipoAvistamiento.Estado;
        }
    }
}
