using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.Items
{
    public class SelectItemsOuput
    {
        public List<Usuario> ItemsUsuario { get; set; }
    }
}
