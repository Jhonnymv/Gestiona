using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class AvistamientoFlora
    {
        public int Id { get; set; }
        public string IdFicha { get; set; }
        public DateTime Fechavalidacion { get; set; }
        public Zona Zona { get; set; }
        public string Ubicacion_Este { get; set; }
        public string Ubicacion_Norte { get; set; }
        public string Ubicacion_Altitud { get; set; }
        public string Geneto { get; set; }
        public string Rameto { get; set; }
        public string Condicion_Reubicacion { get; set; }
        public string Sustrato { get; set; }
        public decimal Cobertura_Diametro_Mayor { get; set; }
        public decimal Cobertura_Diametro_Menor { get; set; }
        public decimal Cobertura_Diametro_Altura { get; set; }
        public string? Fotos { get; set; }

        public AvistamientoFlora()
        {

        }
    }
}
