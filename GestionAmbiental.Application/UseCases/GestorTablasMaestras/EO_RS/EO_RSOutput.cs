using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS
{
    public class EO_RSOutput
    {
        public int Id_EO_RS { get; set; }
        public string Razon_Social { get; set; }
        public string Registro { get; set; }
        public DateTime Fecha_Vencimiento_Registro { get; set; }
        public string Estado { get; set; }

        public EO_RSOutput()
        {
        }
        public EO_RSOutput(Domain.ValueObjects.EO_RS eo_rs)
        {
            Id_EO_RS = eo_rs.Id_EO_RS;
            Razon_Social = eo_rs.Razon_Social;
            Registro = eo_rs.Registro;
            Fecha_Vencimiento_Registro = eo_rs.Fecha_Vencimiento_Registro;
            Estado = eo_rs.Estado;
        }
    }
}
