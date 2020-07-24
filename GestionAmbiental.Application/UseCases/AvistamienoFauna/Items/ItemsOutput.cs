using GestionAmbiental.Domain.AvistamientoFauna.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.Items
{
    public class ItemsOutput
    {
        public List<TipoFauna> TiposFauna { get; set; }
        public List<NombreComunFauna> NombresComunes { get; set; }
        public List<TipoAvistamiento> TiposAvistamiento { get; set; }
        public List<EstadoFauna> EstadosFauna { get; set; }
        public List<AreaTrabajo> AreasTrabajo { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Usuario> Supervisores { get; set; }
    }
}
