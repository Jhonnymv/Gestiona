using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua
{
    public class ResolucionConsumoAguaOutput
    {
        public int IdResolucionConsumoAgua { get; set; }
        public string Resolucion { get; set; }
        public string Anio { get; set; }
        public string TipoUso { get; set; }
        public string ClaseDerecho { get; set; }
        public int IdFuenteAgua { get; set; }
        public string FuenteAgua { get; set; }
        public double VolumenOtorgado { get; set; }
        public string Actividad { get; set; }
        public string Estado { get; set; }
        public ResolucionConsumoAguaOutput() { }
        public ResolucionConsumoAguaOutput(Domain.ConsumoAgua.ResolucionConsumoAgua resolucion) {
            IdResolucionConsumoAgua = resolucion.IdResolucionConsumoAgua;
            Resolucion = resolucion.Resolucion;
            Anio = resolucion.Anio;
            TipoUso = resolucion.TipoUso;
            ClaseDerecho = resolucion.ClaseDerecho;
            IdFuenteAgua = resolucion.FuenteAgua.IdFuenteAgua;
            FuenteAgua = resolucion.FuenteAgua.Descripcion;
            VolumenOtorgado = resolucion.VolumenOtorgado;
            Actividad = resolucion.Actividad;
            Estado = resolucion.Estado;
        }
    }
}
