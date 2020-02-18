using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro
{
    public class ItemsFiltroOutput
    {
        public List<Usuario> AsesoresMAP { get; set; }
        public List<Usuario> AsesoresSSO { get; set; }
        public List<Usuario> RespLevObs { get; set; }
        public List<Gerencia> Gerencias { get; set; }
        public List<Empresa> Empresas { get; set; }
    }
}
