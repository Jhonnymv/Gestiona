using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora
{
    public class NombreCientifico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NombreComun { get; set; }
        public string Descripcion { get; set; }
        public NombreCientifico()
        {

        }
    }
}
