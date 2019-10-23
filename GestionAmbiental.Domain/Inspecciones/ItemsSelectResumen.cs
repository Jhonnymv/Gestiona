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
        public List<Empresa_Contratista> EmpContratMarcobre { get; set; }
        public List<Empresa_Contratista> EmpContratEPCM { get; set; }
        public List<Tipo_Condicion> TipoCondicion { get; set; }
    }
}
