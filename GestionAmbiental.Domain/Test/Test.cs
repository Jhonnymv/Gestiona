using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Test
{
    public class Test
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? Numero { get; set; }
        public string Evidencia { get; set; }
        public TestLista Test_Lista { get; set; }
        public double? Decimal { get; set; }
        public string Estado { get; set; }
        public Test()
        {

        }
    }    
}
