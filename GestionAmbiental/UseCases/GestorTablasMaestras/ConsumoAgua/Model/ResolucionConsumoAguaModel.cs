using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class ResolucionConsumoAguaModel
    {
        public int IdResolucionConsumoAgua { get; set; }
        public string Resolucion { get; set; }
        public string Anio { get; set; }
        public string TipoUso { get; set; }
        public string ClaseDerecho { get; set; }
        public int IdFuenteAgua { get; set; }
        public double VolumenOtorgado { get; set; }
        public string Actividad { get; set; }
        public string Estado { get; set; }
    }
}
