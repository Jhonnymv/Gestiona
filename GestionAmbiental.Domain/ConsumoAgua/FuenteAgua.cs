using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ConsumoAgua
{
    public class FuenteAgua
    {
        public int IdFuenteAgua { get; set; }
        public OrigenAgua OrigenAgua { get; set; }
        public string Descripcion { get; set; }
        public string Coordenadas_UTM { get; set; }
        public string Estado { get; set; }
    }
}
