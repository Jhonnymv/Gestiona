using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class ItemsSelectResumen
    {
        public List<Gerencia> GerenciaEjecutora { get; set; }
        public List<Gerencia> GerenciasEPCM { get; set; }
        public List<Empresa> EmpContratMarcobre { get; set; }
        public List<Empresa> EmpContratEPCM { get; set; }
        public List<TipoCondicion> TipoCondicion { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<Tipo_Inspeccion>TiposInspeccion { get; set; }
    }
}
