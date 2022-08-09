using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAgua
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Cod_Estacion { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public Usuario Responsable { get; set; }
        public string TemperaturaResult { get; set; }
        public string TemperaturaEquipo { get; set; }
        public string PhResult { get; set; }
        public string PhEquipo { get; set; }
        public string ConductividadResult { get; set; }
        public string ConductividadEquipo { get; set; }
        public string OxigenoResult { get; set; }
        public string OxigenoEquipo { get; set; }
        public string TurbiedadResult { get; set; }
        public string TurbiedadEquipo { get; set; }
        public string CloroResult { get; set; }
        public string CloroEquipo { get; set; }

        public CalidadAgua()
        {

        }
    }
}
