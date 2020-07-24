using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna
{
    public class LogAvistamientoFauna
    {
        public int IdLogAvistamientoFauna { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int IdUsuario { get; set; }
        public string Bkp { get; set; }
        public int IdAvistamientoFauna { get; set; }
    }
}
