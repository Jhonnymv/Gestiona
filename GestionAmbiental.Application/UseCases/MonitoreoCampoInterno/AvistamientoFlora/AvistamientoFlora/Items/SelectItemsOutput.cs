using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.Items
{
    public class SelectItemsOutput
    {
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico> ItemsNombreCientifico { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona> ItemsZona { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia> ItemsFenologia { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor> ItemsVigor { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud> ItemsSalud { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso>  ItemsUso { get; set; }
        public List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada> ItemsFaunaAsociada { get; set; }
        public List<Usuario> ItemsUsuario { get; internal set; }
    }
}
