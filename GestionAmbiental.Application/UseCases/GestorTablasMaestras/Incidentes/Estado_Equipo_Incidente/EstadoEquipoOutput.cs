using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente
{
    public class EstadoEquipoOutput
    {
        public int Id_Estado_Equipo_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public EstadoEquipoOutput()
        {
        }
        public EstadoEquipoOutput(Domain.Incidentes.Items.Estado_Equipo_Incidente estado)
        {
            Id_Estado_Equipo_Incidente = estado.Id_Estado_Equipo_Incidente.Value;
            Descripcion = estado.Descripcion;
            Estado = estado.Estado;
        }
    }
}
