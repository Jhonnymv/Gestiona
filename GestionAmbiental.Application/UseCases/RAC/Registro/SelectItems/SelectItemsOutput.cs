using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.SelectItems
{
    public class SelectItemsOutput
    {
        public List<Empresa> ItemsEmpresa { get; set; }
        public List<Gerencia> ItemsGerencia { get; set; }
        public List<TipoCondicion> ItemsTiposCondicion { get; set; }
        public List<Usuario> ItemsUsuarios { get; set; }
        public List<Usuario> ItemsAsesoresSSO { get; set; }
        public List<Usuario> ItemsAsesoresMA { get; set; }
    }
}
