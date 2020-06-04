using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion
{
    public class LugarDisposicionOutput
    {
        public int Id_Lugar_Disposicion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public LugarDisposicionOutput() { }
        public LugarDisposicionOutput(Domain.Incidentes.Items.Lugar_Disposicion lugar) {
            Id_Lugar_Disposicion = lugar.Id_Lugar_Disposicion.Value;
            Descripcion = lugar.Descripcion;
            Estado = lugar.Estado;
        }
    }
}
