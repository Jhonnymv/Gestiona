using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.CargaExcel
{
    public class ConsumoAguaModel
    {
        public DateTime Fecha { get; set; }
        public double TiempoBombeo { get; set; }
        public double LecturaInicial { get; set; }
        public double LecturaFinal { get; set; }
        public int Fila { get; set; }
    }
}
