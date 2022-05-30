using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento
{
    public class NombreCientificoAvistamientoOuput
    {
        public int Id { get; set; }
        public int Id_Avistamiento { get; set; }
        public int Id_NombreCientifico { get; set; }
        public string? NombreCientifico { get; set; }
        public string? NombreComun { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
