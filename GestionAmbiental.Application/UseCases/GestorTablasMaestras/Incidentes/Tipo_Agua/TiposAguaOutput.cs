using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua
{
    public class TiposAguaOutput
    {
        public List<TipoAguaOutput> Tipos { get; set; }
        public int NTipos { get; set; }
    }
}
