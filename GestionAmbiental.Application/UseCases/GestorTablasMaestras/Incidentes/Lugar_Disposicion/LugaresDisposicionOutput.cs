using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion
{
    public class LugaresDisposicionOutput
    {
        public List<LugarDisposicionOutput> Lugares { get; set; }
        public int NLugares { get; set; }
    }
}
