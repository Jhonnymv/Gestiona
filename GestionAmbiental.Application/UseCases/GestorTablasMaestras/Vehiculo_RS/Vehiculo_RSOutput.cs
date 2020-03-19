using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS
{
    public class Vehiculo_RSOutput
    {
        public int Id_Vehiculo_RS { get; set; }
        public string Placa { get; set; }
        public double Capacidad { get; set; }
        public string Estado { get; set; }

        public Vehiculo_RSOutput()
        {
        }
        public Vehiculo_RSOutput(Domain.ValueObjects.Vehiculo_RS vehiculo)
        {
            Id_Vehiculo_RS = vehiculo.Id_Vehiculo_RS;
            Placa = vehiculo.Placa;
            Capacidad = vehiculo.Capacidad;
            Estado = vehiculo.Estado;
        }
    }
}
