using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Reportes
{
    public class Dimension
    {
        public string Nombre { get; set; }
        public double Valor { get; set; }
        public int Index { get; set; }
        public Dimension() { }
        public Dimension(string nombre, double valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
        public Dimension(string nombre, double valor,int index)
        {
            Nombre = nombre;
            Valor = valor;
            Index = index;
        }
    }
}
