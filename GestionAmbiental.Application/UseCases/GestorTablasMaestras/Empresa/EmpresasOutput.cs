using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa
{
    public class EmpresasOutput
    {
        public List<EmpresaOutput> Empresas { get; set; }
        public int nEmpresas { get; set; }
    }
}
