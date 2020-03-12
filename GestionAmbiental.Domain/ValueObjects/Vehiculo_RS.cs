using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Vehiculo_RS
    {
        public int Id_Vehiculo_RS { get; set; }
        public string Placa { get; set; }
        public double Capacidad { get; set; }
        public string Estado { get; set; }
        public Vehiculo_RS()
        {
        }
        public Vehiculo_RS(int id_Vehiculo_RS, string placa, double capacidad, string estado)
        {
            Id_Vehiculo_RS = id_Vehiculo_RS;
            Placa = placa;
            Capacidad = capacidad;
            Estado = estado;
        }

    }
}
