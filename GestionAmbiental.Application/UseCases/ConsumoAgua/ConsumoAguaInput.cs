using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua
{
    public class ConsumoAguaInput
    {
        public int IdResolucionConsumoAgua { get; set; }
        public DateTime Fecha { get; set; }
        public double TiempoBombeo { get; set; }
        public double LecturaInicial { get; set; }
        public double LecturaFinal { get; set; }
        public double VolumenBombeado { get; set; }
        public double CaudalPromedio { get; set; }
        public int IdUsuario { get; set; }
        public string Estado { get; set; }
    }
}
