using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua
{
    public class OrigenAguaOutput
    {
        public int IdOrigenAgua { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public OrigenAguaOutput() { }
        public OrigenAguaOutput(Domain.ConsumoAgua.OrigenAgua origen) {
            IdOrigenAgua = origen.IdOrigenAgua;
            Descripcion = origen.Descripcion;
            Estado = origen.Estado;
        }
    }
}
