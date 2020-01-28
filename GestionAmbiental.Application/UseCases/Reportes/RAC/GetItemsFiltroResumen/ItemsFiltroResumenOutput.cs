using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetItemsFiltroResumen
{
    public class ItemsFiltroResumenOutput
    {
        public List<Gerencia> Gerencias { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}
