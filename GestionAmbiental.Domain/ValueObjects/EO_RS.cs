using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class EO_RS
    {
        public int Id_EO_RS { get; set; }
        public string Razon_Social { get; set; }
        public string Estado { get; set; }

        public EO_RS()
        {
        }

        public EO_RS(int id_EO_RS, string razon_Social, string estado)
        {
            Id_EO_RS = id_EO_RS;
            Razon_Social = razon_Social;
            Estado = estado;
        }
    }
}
