using GestionAmbiental.Domain.SGA_Documentos;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.Items
{
    public class SelectItemsOutput
    {
        public List<TipoDocumento> ItemsTipoDocumento { get; set; }
        public List<Condicion> ItemsCondicion { get; set; }
        public List<Usuario> ItemsUsuarios { get; set; }
    }
}
