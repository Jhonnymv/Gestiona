using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia
{
    public class GerenciaOutput
    {
        public int IdGerencia { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public GerenciaOutput()
        {
        }

        public GerenciaOutput(Domain.ValueObjects.Gerencia gerencia)
        {
            IdGerencia = gerencia.IdGerencia;
            Descripcion = gerencia.Descripcion;
            Estado = gerencia.Estado;
        }
    }
}
