using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia
{
    public class GerenciaOutput
    {
        public int IdGerencia { get; set; }
        public string Descripcion { get; set; }

        public GerenciaOutput()
        {
        }

        public GerenciaOutput(GestionAmbiental.Domain.ValueObjects.Gerencia gerencia)
        {
            IdGerencia = gerencia.IdGerencia;
            Descripcion = gerencia.Descripcion;
        }
    }
}
