using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ConsumoAgua
{
    public class LogConsumoAgua
    {
        public int IdLogConsumoAgua { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int IdUsuario { get; set; }
        public string Bkp { get; set; }
        public int IdConsumoAgua { get; set; }
    }
}
