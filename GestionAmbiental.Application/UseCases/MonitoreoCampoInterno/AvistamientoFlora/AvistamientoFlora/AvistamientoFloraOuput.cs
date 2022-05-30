using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora
{
    public class AvistamientoFloraOuput
    {
        public int Id { get; set; }
        public string IdFicha { get; set; }
        public DateTime Fechavalidacion { get; set; }
        public int? IdZonaMCIF { get; set; }
        public string NombreZona { get; set; }
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
        public AvistamientoFloraOuput()
        {

        }

        public AvistamientoFloraOuput(Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora AvistamientoFlora)
        {
            Id= AvistamientoFlora.Id;
            IdFicha= AvistamientoFlora.IdFicha;
            IdZonaMCIF = AvistamientoFlora.Zona.Id;
            NombreZona = AvistamientoFlora.Zona.Nombre;
            Ubicacion_Este = AvistamientoFlora.Ubicacion_Este;
            Ubicacion_Norte = AvistamientoFlora.Ubicacion_Norte;
            Ubicacion_Altitud=AvistamientoFlora.Ubicacion_Altitud;
            Geneto = AvistamientoFlora.Geneto;
            Rameto= AvistamientoFlora.Rameto;
            Condicion_Reubicacion= AvistamientoFlora.Condicion_Reubicacion;
            Sustrato = AvistamientoFlora.Sustrato;
            Cobertura_Diametro_Mayor = AvistamientoFlora.Cobertura_Diametro_Mayor;
            Cobertura_Diametro_Menor = AvistamientoFlora.Cobertura_Diametro_Menor;
            Cobertura_Diametro_Altura = AvistamientoFlora.Cobertura_Diametro_Altura;
            Fotos = AvistamientoFlora.Fotos;

        }
    }
}
