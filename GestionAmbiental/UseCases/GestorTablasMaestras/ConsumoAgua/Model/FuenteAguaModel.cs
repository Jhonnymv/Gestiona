using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class FuenteAguaModel
    {
        public int IdFuenteAgua { get; set; }
        public int IdOrigenAgua { get; set; }
        public string Descripcion { get; set; }
        public string Coordenadas_UTM { get; set; }
        public string Estado { get; set; }
    }
}
