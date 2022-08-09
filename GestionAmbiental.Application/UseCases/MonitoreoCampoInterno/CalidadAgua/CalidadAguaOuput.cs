using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAguaOuput
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string  Hora { get; set; }
        public string Cod_Estacion { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public int Id_Usuario { get; set; }
        public string NombreResponsable { get; set; }
        public string ApellidoResponsable { get; set; }
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
    }
}
