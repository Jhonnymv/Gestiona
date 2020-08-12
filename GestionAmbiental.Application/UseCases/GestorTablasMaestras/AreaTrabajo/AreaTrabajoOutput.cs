using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo
{
    public class AreaTrabajoOutput
    {
        public int IdAreaTrabajo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public AreaTrabajoOutput()
        {
        }
        public AreaTrabajoOutput(Domain.ValueObjects.AreaTrabajo areaTrabajo)
        {
            IdAreaTrabajo = areaTrabajo.IdAreaTrabajo;
            Descripcion = areaTrabajo.Descripcion;
            Estado = areaTrabajo.Estado;
        }
    }
}
