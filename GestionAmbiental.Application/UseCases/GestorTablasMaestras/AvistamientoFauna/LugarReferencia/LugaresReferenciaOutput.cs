using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia
{
    public class LugaresReferenciaOutput
    {
        public List<LugarReferenciaOutput> LugaresReferencias { get; set; }
        public int NLugares { get; set; }
    }
}
