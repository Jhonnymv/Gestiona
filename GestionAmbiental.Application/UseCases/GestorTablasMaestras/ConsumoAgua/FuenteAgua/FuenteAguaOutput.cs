using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua
{
    public class FuenteAguaOutput
    {
        public int IdFuenteAgua { get; set; }
        public int IdOrigenAgua { get; set; }
        public string OrigenAgua { get; set; }
        public string Descripcion { get; set; }
        public string Coordenadas_UTM { get; set; }
        public string Estado { get; set; }
        public FuenteAguaOutput() { }
        public FuenteAguaOutput(Domain.ConsumoAgua.FuenteAgua fuente) {
            IdFuenteAgua = fuente.IdFuenteAgua;
            IdOrigenAgua = fuente.OrigenAgua.IdOrigenAgua;
            OrigenAgua = fuente.OrigenAgua.Descripcion;
            Descripcion = fuente.Descripcion;
            Coordenadas_UTM = fuente.Coordenadas_UTM;
            Estado = fuente.Estado;
        }
    }
}
