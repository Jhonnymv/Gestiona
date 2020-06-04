using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo
{
    public class TipoSueloOutput
    {
        public int Id_Tipo_Suelo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoSueloOutput()
        {
        }
        public TipoSueloOutput(Domain.Incidentes.Items.Tipo_Suelo tipo)
        {
            Id_Tipo_Suelo = tipo.Id_Tipo_Suelo.Value;
            Descripcion = tipo.Descripcion;
            Estado = tipo.Estado;
        }
    }
}
